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
    public class CustomerAssessResultSV
    {
        private CustomerAssessResultDA CustomerAssessResultDA = new CustomerAssessResultDA();
        /// <summary>
        /// Lấy danh sách kết quả đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerAssessResultItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerAssessResultDA.Repository;
            var CustomerAssessResult = CustomerAssessResultDA.GetQuery()
                        .Select(item => new CustomerAssessResultItem()
                        {
                            ID = item.ID,
                            AuditID = item.CustomerAssessID,
                            CriteriaID = item.CustomerCriteriaID,
                            CustomerID = item.CustomerID,
                            Point = item.Point,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerAssessResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerAssessResultItem> GetByCustomerID(int page, int pageSize, out int totalCount, int customerID)
        {
            var dbo = CustomerAssessResultDA.Repository;
            var CustomerAssessResult = dbo.CustomerAssessResults.Where(i => i.CustomerID == customerID)
                        .Select(item => new CustomerAssessResultItem()
                        {
                            ID = item.CustomerAssess.ID,
                            Name = item.CustomerAssess.Name,
                        })
                     .OrderByDescending(item => item.Name)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            ;
            return CustomerAssessResult;
        }
        /// <summary>
        /// Lấy kết quả đánh giá theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerAssessResultItem GetById(int Id)
        {
            var CustomerAssessResult = CustomerAssessResultDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerAssessResultItem()
                        {
                            ID = item.ID,
                            AuditID = item.CustomerAssessID,
                            CriteriaID = item.CustomerCriteriaID,
                            CustomerID = item.CustomerID,
                            Point = item.Point,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerAssessResult;
        }

        /// <summary>
        /// Cập nhật kết quả đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerAssessResultItem item, int userID)
        {
            var CustomerAssessResult = CustomerAssessResultDA.GetById(item.ID);
            CustomerAssessResult.CustomerAssessID = item.AuditID;
            CustomerAssessResult.CustomerCriteriaID = item.CriteriaID;
            CustomerAssessResult.CustomerID = item.CustomerID;
            CustomerAssessResult.Point = item.Point;
            CustomerAssessResult.UpdateAt = DateTime.Now;
            CustomerAssessResult.UpdateBy = userID;
            CustomerAssessResultDA.Save();
        }
        /// <summary>
        /// Thêm mới kết quả đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerAssessResultItem item, int userID)
        {
            var CustomerAssessResult = new CustomerAssessResult()
            {
                CustomerAssessID = item.AuditID,
                CustomerCriteriaID = item.CriteriaID,
                CustomerID = item.CustomerID,
                Point = item.Point,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerAssessResultDA.Insert(CustomerAssessResult);
            CustomerAssessResultDA.Save();
        }
        /// <summary>
        /// Xóa kết quả đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerAssessResultDA.Delete(id);
            CustomerAssessResultDA.Save();
        }
        public List<int> getIDsbyParentID(int id)
        {
            List<int> resut = new List<int>();
            var dbo = CustomerAssessResultDA.Repository;
            var Ids = dbo.CustomerCriteriaCategories.Where(i => i.ParentID == id).Select(i => i.ID).ToList();
            resut.Add(id);
            if (Ids.Count() > 0)
            {
                foreach (var item in Ids)
                {
                    resut.AddRange(getIDsbyParentID(item));
                }
            }
            return resut;
        }
        public List<CustomerAssessResultItem> GetCriteriasByParentID(int parentId, int cateId, int auditId, int customerId)
        {
            List<CustomerAssessResultItem> lst = new List<CustomerAssessResultItem>();
            var dbo = CustomerAssessResultDA.Repository;
            var ListCateIDs = getIDsbyParentID(cateId);
            lst = dbo.CustomerCriterias.Where(t =>
                            ListCateIDs.Contains(t.CustomerCriteriaCategoryID) && !t.IsDelete && t.IsActive)
                            .Select(c => new CustomerAssessResultItem
                            {
                                ID = c.ID,
                                Name = c.Name,
                                MinPoint = c.MinPoint,
                                MaxPoint = c.MaxPoint,
                                Point = dbo.CustomerAssessResults.FirstOrDefault(cA => cA.CustomerCriteriaID == c.ID
                                        && cA.CustomerAssessID == auditId
                                        && cA.CustomerID == customerId
                                    ) == null ? 0 : dbo.CustomerAssessResults.FirstOrDefault(cA => cA.CustomerCriteriaID == c.ID
                                            && cA.CustomerAssessID == auditId
                                            && cA.CustomerID == customerId
                                            ).Point,
                                CustomerID = dbo.CustomerAssessResults.FirstOrDefault(cA => cA.CustomerCriteriaID == c.ID
                                        && cA.CustomerAssessID == auditId
                                        && cA.CustomerID == customerId
                                    ) == null ? 0 : dbo.CustomerAssessResults.FirstOrDefault(cA => cA.CustomerCriteriaID == c.ID
                                            && cA.CustomerAssessID == auditId
                                            && cA.CustomerID == customerId
                                    ).CustomerID,
                            })
                           .ToList();
            return lst;
        }
        public CustomerAssessResultItem GetAuditResult(int auditId, int criteriaId, int customerId)
        {
            var dbo = CustomerAssessResultDA.Repository;
            var criteria = dbo.CustomerCriterias.FirstOrDefault(i => i.ID == criteriaId);
            CustomerAssessResultItem obj = new CustomerAssessResultItem()
            {
                AuditID = auditId,
                CriteriaID = criteriaId,
                CustomerID = customerId,
                MinPoint = criteria == null ? 0 : criteria.MinPoint,
                MaxPoint = criteria == null ? 0 : criteria.MaxPoint
            };
            var result = CustomerAssessResultDA
                .GetQuery(t => t.CustomerAssessID == auditId && t.CustomerCriteriaID == criteriaId && t.CustomerID == customerId).FirstOrDefault();
            if (result != null)
            {
                obj.Point = result.Point;
                obj.CustomerID = result.CustomerID;
            }
            return obj;
        }
        public void InsertOrUpdate(CustomerAssessResultItem obj, int userId)
        {

            var result = CustomerAssessResultDA
                .GetQuery(t => t.CustomerAssessID == obj.AuditID
                    && t.CustomerCriteriaID == obj.CriteriaID
                    && t.CustomerID == obj.CustomerID).FirstOrDefault();
            if (result != null)
            {
                result.Point = (int)obj.Point;
                result.UpdateAt = DateTime.Now;
                result.UpdateBy = userId;
                CustomerAssessResultDA.Update(result);
            }
            else
            {
                CustomerAssessResult objI = new CustomerAssessResult();
                objI.CustomerCriteriaID = obj.CriteriaID;
                objI.CustomerAssessID = obj.AuditID;
                objI.CustomerID = obj.CustomerID;
                objI.Point = (int)obj.Point;
                objI.CreateAt = DateTime.Now;
                objI.CreateBy = userId;
                CustomerAssessResultDA.Insert(objI);
            }
            CustomerAssessResultDA.Save();
        }
        public void SaveMailAudit(List<CustomerAssessResultItem> objs, int AuditID, int CustomerID)
        {
            foreach (var obj in objs)
            {
                var result = CustomerAssessResultDA.GetQuery(t => t.CustomerAssessID == AuditID
                                                       && t.CustomerCriteriaID == obj.CriteriaID
                                                       && t.CustomerID == CustomerID).FirstOrDefault();
                if (result != null)
                {
                    result.Point = (int)obj.Point;
                    result.UpdateAt = DateTime.Now;
                    CustomerAssessResultDA.Update(result);
                }
                else
                {
                    CustomerAssessResult objI = new CustomerAssessResult();
                    objI.CustomerCriteriaID = obj.CriteriaID;
                    objI.CustomerAssessID = AuditID;
                    objI.CustomerID = CustomerID;
                    objI.Point = obj.Point;
                    objI.CreateAt = DateTime.Now;
                    CustomerAssessResultDA.Insert(objI);
                }
                CustomerAssessResultDA.Save();
            }
        }
        public void Delete(int auditId, int criteriaId, int customerId)
        {
            var result = CustomerAssessResultDA
                .GetQuery(t => t.CustomerAssessID == auditId && t.CustomerCriteriaID == criteriaId && t.CustomerID == customerId).FirstOrDefault();
            if (result != null)
                CustomerAssessResultDA.Delete(result);
            CustomerAssessResultDA.Save();

        }
    }
}
