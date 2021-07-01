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
    public class CustomerSurveyObjectSV
    {
        private CustomerSurveyObjectDA CustomerSurveyObjectDA = new CustomerSurveyObjectDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerSurveyObjectItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerSurveyObjectDA.Repository;
            var CustomerSurveyObject = CustomerSurveyObjectDA.GetQuery()
                        .Select(item => new CustomerSurveyObjectItem()
                        {
                            ID = item.ID,
                            SurveyID = item.CustomerSurveyID,
                            CategoryID = item.CustomerCategoryID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerSurveyObject;

        }
        /// <summary>
        /// Lấy danh sách các nhóm khách hàng cần khảo sát
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="SurveyID"></param>
        /// <returns></returns>
        public List<CustomerSurveyObjectItem> GetAllSurveyObject(int page, int pageSize, out int totalCount, int SurveyID)
        {
            var dbo = CustomerSurveyObjectDA.Repository;
            var SurveyObjectList= dbo.CustomerCategories.Where(i => i.IsDelete == false)
                        .Select(item => new CustomerSurveyObjectItem()
                        {
                            ID = dbo.CustomerSurveyObjects.FirstOrDefault(i => i.CustomerCategoryID == item.ID && i.CustomerSurveyID == SurveyID).ID,
                            SurveyID = SurveyID,
                            CategoryID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            IsSelect = dbo.CustomerSurveyObjects.FirstOrDefault(i => i.CustomerCategoryID == item.ID && i.CustomerSurveyID == SurveyID) != null
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return SurveyObjectList;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerSurveyObjectItem GetById(int Id)
        {
            var CustomerSurveyObject = CustomerSurveyObjectDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerSurveyObjectItem()
                        {
                            ID = item.ID,
                            SurveyID = item.CustomerSurveyID,
                            CategoryID = item.CustomerCategoryID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerSurveyObject;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerSurveyObjectItem item, int userID)
        {
            var CustomerSurveyObject = CustomerSurveyObjectDA.GetById(item.ID);
            CustomerSurveyObject.CustomerSurveyID = item.SurveyID;
            CustomerSurveyObject.CustomerCategoryID = item.CategoryID;
            CustomerSurveyObject.UpdateAt = DateTime.Now;
            CustomerSurveyObject.UpdateBy = userID;
            CustomerSurveyObjectDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerSurveyObjectItem item, int userID)
        {
            var CustomerSurveyObject = new CustomerSurveyObject()
            {
                CustomerSurveyID = item.SurveyID,
                CustomerCategoryID = item.CategoryID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerSurveyObjectDA.Insert(CustomerSurveyObject);
            CustomerSurveyObjectDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 30/01/2015
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerSurveyObjectDA.Delete(id);
            CustomerSurveyObjectDA.Save();
        }
    }
}
