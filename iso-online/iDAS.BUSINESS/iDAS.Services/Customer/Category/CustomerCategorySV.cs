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
    public class CustomerCategorySV
    {
        private CustomerCategoryDA CustomerCategoryDA = new CustomerCategoryDA();
        public void UpdateRoot(int userId)
        {
            var dataRoot = CustomerCategoryDA.GetQuery(i => i.ParentID == 0);
            if (dataRoot.FirstOrDefault() == null)
            {
                var root = new CustomerCategoryItem()
                {
                    Name = "Nhóm khách hàng",
                    ParentID = 0,
                    IsDelete = false,
                };
                Insert(root, userId);
            }
        }
        private IEnumerable<CustomerCategory> getChilds(IEnumerable<CustomerCategory> e, int? id)
        {
            var customerCategory = e.Where(a => a.ParentID == id);
            var customerCategoryFirst = e.Where(a => a.ID == id);
            customerCategoryFirst.Concat(customerCategory);
            return customerCategoryFirst.Concat(customerCategory.SelectMany(a => getChilds(e, a.ID)));
        }
        private IEnumerable<CustomerCategory> getParents(IEnumerable<CustomerCategory> e, int? id)
        {
            var listParentNext = e.Where(a => a.ID == id);
            var result = listParentNext.Concat(listParentNext.SelectMany(a => getParents(e, a.ParentID)));
            return result;
        }
        /// <summary>
        /// Lấy danh sách nhóm khách hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerCategoryItem> GetAll(int page, int pageSize, out int totalCount, int userID)
        {
            var dbo = CustomerCategoryDA.Repository;
            var CustomerCatePermissions = CustomerCategoryDA.GetQuery(//i => i.EM.EmployeeID == userID
                );
            foreach (var Cate in CustomerCatePermissions)
            {
                CustomerCatePermissions.Concat(getChilds(dbo.CustomerCategories//.Where(i => i.EmployeeID == userID)
                    , Cate.ID)).Distinct();
            }
            var groupCustomer = CustomerCatePermissions
                //dbo.CustomerCategories
                        .Select(item => new CustomerCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            // IsCreate = item.IsCreate,
                            ParentID = item.ParentID,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return groupCustomer;
        }
        /// <summary>
        /// Lấy danh sách khách hàng cho combobox
        /// </summary>
        /// <returns></returns>
        public List<CustomerCategoryItem> GetAll(int userID)
        {
            var dbo = CustomerCategoryDA.Repository;
            // var CustomerCategoryPermission = CustomerCategoryDA.GetQuery(i => i.EmployeeID == userID).FirstOrDefault();
            var groupCustomer = //getChilds(CustomerCategoryDA.GetQuery(i => i.EmployeeID == userID), CustomerCategoryPermission.ID)
                        dbo.CustomerCategories
                        .Select(item => new CustomerCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            //  IsCreate = item.IsCreate,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            ;
            return groupCustomer;
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerCategoryItem> GetAllGroupCustomerActive(int page, int pageSize, out int totalCount, int userID)
        {
            var dbo = CustomerCategoryDA.Repository;
            var CustomerCategoryPermission = CustomerCategoryDA.GetQuery(//i => i.EmployeeID == userID
                ).FirstOrDefault();
            var groupCustomer = dbo.CustomerCategories
                        .Select(item => new CustomerCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            //  IsCreate = item.IsCreate,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return groupCustomer;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<CustomerCategoryItem> GetTreeData(int nodeId, int employeeId, bool isAdministrator)
        {
            var dbo = CustomerCategoryDA.Repository;
            if (isAdministrator)
            {
                var groupCustomer = CustomerCategoryDA.GetQuery(i => (i.ParentID != null && i.ParentID == nodeId))
                     .Select(item => new CustomerCategoryItem()
                     {
                         ID = item.ID,
                         ParentID = item.ParentID,
                         Name = item.Name,
                         Note = item.Note,
                         IsDelete = item.IsDelete,
                         Leaf = dbo.CustomerCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                     })
                     .ToList();
                return groupCustomer;
            }
            else
            {
                var depIds = dbo.HumanEmployees.Where(i => i.ID == employeeId)
                                .SelectMany(i => i.HumanOrganizations)
                                .Select(i => i.HumanRole)
                                .Select(i => i.HumanDepartment.ID).ToList();
                List<CustomerCategory> CategoryShow = new List<CustomerCategory>();
                List<CustomerCategory> parentCategoryOfEmployee = new List<CustomerCategory>();
                List<CustomerCategory> childCategoryOfEmployee = new List<CustomerCategory>();
                List<CustomerCategory> categoryOfEmployee = dbo.CustomerCategoryDepartments.Where(i => i.HumanDepartmentID != null && depIds.Contains((int)i.HumanDepartmentID)
                    ).Select(i => i.CustomerCategory).ToList();
                foreach (var Cate in categoryOfEmployee)
                {
                    parentCategoryOfEmployee.AddRange(getParents(CustomerCategoryDA.GetQuery(), Cate.ID));
                }
                foreach (var Cate in categoryOfEmployee)
                {
                    childCategoryOfEmployee.AddRange(getChilds(CustomerCategoryDA.GetQuery(), Cate.ID));
                }
                parentCategoryOfEmployee = parentCategoryOfEmployee.Where(item => !childCategoryOfEmployee.Contains(item)).ToList();
                CategoryShow.AddRange(parentCategoryOfEmployee);
                CategoryShow.AddRange(childCategoryOfEmployee);
                var groupCustomer = CategoryShow.Distinct()
                       .Where(i => (i.ParentID != null && i.ParentID == nodeId))
                       .Select(item => new CustomerCategoryItem()
                       {
                           ID = item.ID,
                           ParentID = item.ParentID,
                           Name = item.Name,
                           Note = item.Note,
                           IsEmployee = item.CustomerCategoryDepartments == null ? false : categoryOfEmployee.Contains(item),
                           IsParent = !categoryOfEmployee.Select(i => i.ID).Contains(item.ID) && parentCategoryOfEmployee.Select(i => i.ID).Contains(item.ID),
                           IsDelete = item.IsDelete,
                           Leaf = dbo.CustomerCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                       })
                       .ToList();
                return groupCustomer;
            }
        }
        public List<CustomerCategoryItem> GetTreeAllData(int nodeId)
        {
            var dbo = CustomerCategoryDA.Repository;
            var result = new List<CustomerCategoryItem>();
            var groupCustomers = CustomerCategoryDA
                        .GetQuery(i => (i.ParentID != null && i.ParentID == nodeId))
                        .Select(item => new CustomerCategoryItem()
                        {
                            ID = item.ID,
                            ParentID = item.ParentID,
                            Name = item.Name,
                            Note = item.Note,
                            IsDelete = item.IsDelete,
                            UpdateAt  = item.UpdateAt == null? item.CreateAt:item.UpdateAt,
                            Leaf = dbo.CustomerCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                        })
                        .ToList();
            foreach (var group in groupCustomers)
            {
                group.Departments = new HumanDepartmentViewItem()
                            {
                                Name = dbo.CustomerCategoryDepartments.Count(i => i.CustomerCategoryID == group.ID) > 0
                                        ? dbo.CustomerCategoryDepartments.Where(i => i.CustomerCategoryID == group.ID)
                                                    .Select(i => i.HumanDepartment.Name).ToList()
                                                    .Aggregate((current, next) => current + "," + next).ToString()
                                        : "",
                            };
                result.Add(group);
            }
            return result;
        }
        /// <summary>
        /// Lấy nhóm khách hàng theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerCategoryItem GetById(int Id)
        {
            var dbo = CustomerCategoryDA.Repository;
            var result = dbo.CustomerCategories.Where(i => i.ID == Id)
                        .Select(item => new CustomerCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            //   IsCreate = item.IsCreate,
                            Note = item.Note,
                            IsDelete = item.IsDelete,
                        })
                        .First();
            var depts = dbo.CustomerCategoryDepartments.Where(i => i.CustomerCategoryID == Id).ToList();
            result.Departments = new HumanDepartmentViewItem()
                             {
                                 IDs = depts.Select(i => i.HumanDepartmentID).ToList(),
                                 Name = depts.Count == 0 ? "" : depts.Select(i => i.HumanDepartment.Name).Aggregate((current, next) => current + "," + next).ToString(),
                             };
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<CustomerCategoryDetailItem> GetByIds(int[] ids)
        {
            var dbo = CustomerCategoryDA.Repository;
            var results = new List<CustomerCategoryDetailItem>();
            var items = dbo.CustomerCategories.Where(i => ids.Contains(i.ID) && !i.IsDelete)
                            .Select(item => new CustomerCategoryDetailItem()
                            {
                                ID = item.ID,
                            }).ToList();
            if (items != null && items.Count > 0)
                foreach (var item in items)
                {
                    item.Names = getParents(dbo.CustomerCategories, item.ID).Select(i => i.Name).ToList();
                    results.Add(item);
                }
            return results;
        }
        /// <summary>
        /// Cập nhật nhóm khách hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerCategoryItem item, int userID)
        {
            var dbo = CustomerCategoryDA.Repository;
            var customerCategory = CustomerCategoryDA.GetById(item.ID);
            customerCategory.ID = item.ID;
            customerCategory.Name = item.Name;
            customerCategory.Note = item.Note;
            customerCategory.IsDelete = item.IsDelete;
            customerCategory.CreateAt = item.CreateAt;
            customerCategory.CreateBy = item.CreateBy;
            customerCategory.UpdateAt = DateTime.Now;
            customerCategory.UpdateBy = userID;
            if (item.Departments.IDs.Count >0)
            {
                var oldDepartmentIds = dbo.CustomerCategoryDepartments.Where(i => i.CustomerCategoryID == item.ID).Select(i => i.HumanDepartmentID).ToList();
                var departmentIds = item.Departments.IDs;
                if (oldDepartmentIds != null && oldDepartmentIds.Count > 0)
                {
                    var insertIds = departmentIds.Where(i => !oldDepartmentIds.Contains(i));
                    var deleteIds = oldDepartmentIds.Where(i => !departmentIds.Contains(i));
                    foreach (var insertId in insertIds)
                    {
                        customerCategory.CustomerCategoryDepartments.Add(new CustomerCategoryDepartment()
                        {
                            HumanDepartmentID = insertId,
                            CustomerCategoryID = item.ID,
                            CreateAt = DateTime.Now,
                            CreateBy = userID
                        });
                    }
                    dbo.CustomerCategoryDepartments.RemoveRange(
                    dbo.CustomerCategoryDepartments.Where(i => deleteIds.Contains(i.HumanDepartmentID) && i.CustomerCategoryID == item.ID));
                }
                else
                {
                    foreach (var deptId in departmentIds)
                    {
                        dbo.CustomerCategoryDepartments.Add(new CustomerCategoryDepartment()
                        {
                            HumanDepartmentID = deptId,
                            CustomerCategoryID = item.ID,
                            CreateAt = DateTime.Now,
                            CreateBy = userID
                        });
                    }
                }
            }
            else
            {
                dbo.CustomerCategoryDepartments.RemoveRange(dbo.CustomerCategoryDepartments.Where(i => i.CustomerCategoryID == item.ID));
            }
            CustomerCategoryDA.Save();
        }
        /// <summary>
        /// Thêm mới nhóm khách hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(CustomerCategoryItem item, int userID)
        {
            var customerCategory = new CustomerCategory()
            {
                ID = item.ID,
                ParentID = item.ParentID,
                Name = item.Name,
                Note = item.Note,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            if (item.Departments != null && item.Departments.IDs.Count >0)
            {
                var deptIds = item.Departments.IDs;
                foreach (var deptId in deptIds)
                {
                    customerCategory.CustomerCategoryDepartments.Add(new CustomerCategoryDepartment()
                    {
                        HumanDepartmentID = deptId,
                        CreateAt = DateTime.Now,
                        CreateBy = userID
                    });
                }
            }
            CustomerCategoryDA.Insert(customerCategory);
            CustomerCategoryDA.Save();
            return customerCategory.ID;
        }
        /// <summary>
        /// Xóa nhóm khách hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var dbo = CustomerCategoryDA.Repository;
            if (dbo.CustomerCategories.Where(i => i.ParentID == id).FirstOrDefault() == null)
            {
                dbo.CustomerCategoryDepartments.RemoveRange(dbo.CustomerCategoryDepartments.Where(i => i.CustomerCategoryID == id));
                dbo.CustomerCategories.Remove(dbo.CustomerCategories.Where(i => i.ID == id).FirstOrDefault());
                dbo.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
