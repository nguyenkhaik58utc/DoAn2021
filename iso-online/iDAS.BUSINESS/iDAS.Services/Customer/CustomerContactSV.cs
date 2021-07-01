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
    public class CustomerContactSV
    {
        private CustomerContactDA CustomerContactDA = new CustomerContactDA();
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerContactItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerContactDA.Repository;
            var CustomerPerson = CustomerContactDA.GetQuery()
                        .Select(item => new CustomerContactItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Birthday = item.Birthday,
                            AttachmentFileID = item.AttachmentFileID,
                            FileName = item.AttachmentFileID == null ? "" : item.AttachmentFile.Name,
                            Address = item.Address,
                            Department = item.Department,
                            Role = item.Role,
                            IsDelete = item.IsDelete
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerPerson;
        }

        public List<CustomerContactItem> GetAll()
        {
            var dbo = CustomerContactDA.Repository;
            var CustomerPerson = CustomerContactDA.GetQuery()
                        .Select(item => new CustomerContactItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Birthday = item.Birthday,
                            //Image = item.Image,
                            Address = item.Address,
                            Department = item.Department,
                            Role = item.Role,
                            IsDelete = item.IsDelete
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            ;
            return CustomerPerson;
        }
        /// <summary>
        /// Lấy danh sách liên hệ theo danh sách khách hàng
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerContactItem> GetByCustomer(int page, int pageSize, out int totalCount, int customerId)
        {
            var dbo = CustomerContactDA.Repository;
            var result = new List<CustomerContactItem>();
            var lscontacts = CustomerContactDA.GetQuery(i => i.CustomerID == customerId)
                                            .Select(i=>i)
                                            .OrderByDescending(item => item.Name)
                                            .Page(page, pageSize, out totalCount)
                                            .ToList();
            if (lscontacts != null)
            {
                foreach (var item in lscontacts)
                {
                    result.Add(new CustomerContactItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Birthday = item.Birthday,
                            AttachmentFileID = item.AttachmentFileID,
                            FileName = item.AttachmentFileID == null ? "" : dbo.AttachmentFiles.Where(i => i.ID == item.AttachmentFileID).Select(i => i.Name).FirstOrDefault(),
                            Address = item.Address,
                            Department = item.Department,
                            Role = item.Role,
                            IsDelete = item.IsDelete
                        });
                }
            }
            return result;
        }
        public List<CustomerContactItem> GetByCustomer(int CustomerID)
        {
            var dbo = CustomerContactDA.Repository;
            var CustomerPerson = CustomerContactDA.GetQuery(i => i.CustomerID == CustomerID)
                        .Select(item => new CustomerContactItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Birthday = item.Birthday,
                            AttachmentFileID = item.AttachmentFileID,
                            FileName = item.AttachmentFileID == null ? "" : item.AttachmentFile.Name,
                            Address = item.Address,
                            Department = item.Department,
                            Role = item.Role,
                            IsDelete = item.IsDelete
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            ;
            return CustomerPerson;
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerContactItem GetById(int Id)
        {
            var CustomerPerson = CustomerContactDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerContactItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Email = item.Email,
                            Phone = item.Phone,
                            Birthday = item.Birthday,
                            AttachmentFileID = item.AttachmentFileID,
                            FileName = item.AttachmentFileID == null ? "" : item.AttachmentFile.Name,
                            Address = item.Address,
                            Note = item.Note,
                            Department = item.Department,
                            Role = item.Role,
                            IsDelete = item.IsDelete,
                        })
                        .First();
            return CustomerPerson;
        }

        /// <summary>
        /// Cập nhật khách hàng 
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerContactItem item, int userID)
        {
            var CustomerPerson = CustomerContactDA.GetById(item.ID);
            CustomerPerson.ID = item.ID;
            CustomerPerson.Name = item.Name;
            CustomerPerson.Email = item.Email;
            CustomerPerson.Phone = item.Phone;
            CustomerPerson.Birthday = item.Birthday;
            CustomerPerson.Address = item.Address;
            CustomerPerson.Note = item.Note;
            CustomerPerson.Department = item.Department;
            CustomerPerson.Role = item.Role;
            CustomerPerson.IsDelete = item.IsDelete;
            if (item.ImageFile != null)
            {
                if (CustomerPerson.AttachmentFileID == null)
                {
                    item.ImageFile.ID = Guid.NewGuid();
                    new FileSV().Insert(item.ImageFile, userID);
                    CustomerPerson.AttachmentFileID = item.ImageFile.ID;
                }
                else
                {
                    item.ImageFile.ID = (Guid)CustomerPerson.AttachmentFileID;
                    FileDA FileDA = new FileDA();
                    var file = FileDA.GetById(item.ImageFile.ID);
                    file.ID = item.ImageFile.ID;
                    file.Name = item.ImageFile.Name;
                    file.Extension = item.ImageFile.Extension;
                    //file.Path = item.Path;
                    file.Size = item.ImageFile.Size;
                    file.Type = item.ImageFile.Type;
                    file.Data = item.ImageFile.Data;
                    file.UpdateAt = DateTime.Now;
                    file.UpdateBy = userID;
                    FileDA.Save();
                    CustomerPerson.AttachmentFileID = file.ID;
                }
            }
            CustomerContactDA.Save();
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerContactItem item, int userID)
        {
            var CustomerPerson = new CustomerContact()
            {
                CustomerID = item.CustomerID,
                Name = item.Name,
                Email = item.Email,
                Phone = item.Phone,
                Birthday = item.Birthday,
                Address = item.Address,
                Note = item.Note,
                Department = item.Department,
                Role = item.Role,
                IsDelete = false,
            };
            if (item.ImageFile != null)
            {
                var imgId = new FileSV().Insert(item.ImageFile, userID);
                CustomerPerson.AttachmentFileID = imgId;
            }
            CustomerContactDA.Insert(CustomerPerson);
            CustomerContactDA.Save();
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerContactDA.Delete(id);
            CustomerContactDA.Save();
        }
    }
}
