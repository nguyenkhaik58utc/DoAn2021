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
    public class CustomerContractSV
    {
        private CustomerContractDA CustomerContractDA = new CustomerContractDA();
        private IEnumerable<CustomerCategory> getChilds(IEnumerable<CustomerCategory> e, List<int> ids)
        {
            var customerCategory = e.Where(a => a.ParentID != null && ids.Contains((int)a.ParentID));
            var customerCategoryFirst = e.Where(a => ids.Contains(a.ID));
            customerCategoryFirst.Concat(customerCategory);
            return customerCategoryFirst.Concat(customerCategory.SelectMany(a => getChilds(e, new List<int> { a.ID })));
        }
        /// <summary>
        /// Lấy danh sách hợp đồng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerContractItem> GetAll(ModelFilter filter)
        {
            var dbo = CustomerContractDA.Repository;
            var CustomerContract = CustomerContractDA.GetQuery()
                        .Select(item => new CustomerContractItem()
                        {
                            ID = item.ID,
                            //CustomerID = item.CustomerID,
                            Code = item.Code,
                            Name = item.Name,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            IsSuccess = item.IsSuccess,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                         .Filter(filter)
                        .ToList();
            return CustomerContract;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<CustomerContractItem> GetByCustomer(int page, int pageSize, out int totalCount, int CustomerID)
        {
            var CustomerContract = CustomerContractDA.GetQuery(i => i.CustomerID == CustomerID)
                                        .Select(item => new CustomerContractItem()
                                        {
                                            ID = item.ID,
                                            Code = item.Code,
                                            CustomerID = item.CustomerID,
                                            CustomerName = item.Customer.Name,
                                            Total = item.Total,
                                            Name = item.Name,
                                            FinishDate = item.FinishDate,
                                            IsFinish = item.IsFinish,
                                            IsPause = item.IsPause,
                                            IsSuccess = item.IsSuccess,
                                            IsSend = item.IsSend,
                                            IsSignReview = item.IsSignReview,
                                            IsSignCompany = item.IsSignCompany,
                                            IsSignCustomer = item.IsSignCustomer,
                                            IsAccept = item.IsAccept,
                                            IsApproval = item.IsApproval,
                                            IsCancel = item.IsCancel,
                                            IsEdit = item.IsEdit,
                                            IsStart = item.IsStart,
                                            CreateAt = item.CreateAt,
                                        })
                                        .OrderByDescending(item => item.CreateAt)
                                        .Page(page, pageSize, out totalCount)
                                        .ToList();
            return CustomerContract;
        }
        private decimal GetRateFinishContract(int contractId)
        {
            var dbo = CustomerContractDA.Repository;
            var s = dbo.ServiceCommandContracts.Where(t => t.CustomerContractID == contractId).Select(t => t.ID).ToArray();
            var q = dbo.ServicePlans.Where(t => s.Contains(t.ServiceCommandContractID.Value) && t.QualityPlan.ParentID != 0).Select(t => t.QualityPlanID).ToArray();
            decimal rate = 0;
            foreach (var item in q)
            {
                var lstTaskID = dbo.QualityPlanTasks.Where(x => x.QualityPlanID == item).Select(x => x.TaskID).ToArray();
                if (dbo.Tasks.Where(x => lstTaskID.Contains(x.ID) && x.IsComplete).Count() != 0)
                {
                    rate += (System.Math.Round((decimal)dbo.Tasks.Where(x => lstTaskID.Contains(x.ID) && x.IsComplete).Count() / (decimal)dbo.Tasks.Where(x => lstTaskID.Contains(x.ID) && !x.IsPause && !x.IsNew).Count(), 2)) * 100;

                }
            }
            return rate != 0 ? System.Math.Round(rate / (decimal)q.Count(), 2) : 0;
        }
        /// <summary>
        /// Lấy danh sách hợp đồng theo mã nhân viên
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<CustomerContractItem> GetByEmployee(ModelFilter filter, int employeeId, bool isAdministrator)
        {
            var dbo = CustomerContractDA.Repository;
            List<int> catePermissIds;
            if (isAdministrator)
            {
                catePermissIds = dbo.CustomerCategories.Where(i=>!i.IsDelete).Select(i=>i.ID).ToList();
            }
            else
            {
                var depIds = dbo.HumanEmployees.Where(i => i.ID == employeeId)
                                .SelectMany(i => i.HumanOrganizations)
                                .Select(i => i.HumanRole)
                                .Select(i => i.HumanDepartment.ID);
                var CategoryIDsForUser = dbo.CustomerCategoryDepartments
                                                .Where(i => i.HumanDepartment != null && depIds.Contains((int)i.HumanDepartmentID))
                                                .Select(i => i.CustomerCategoryID).Distinct().ToList();
                // Những nhóm khách hàng mà người đăng nhập có quyền truy vấn
                catePermissIds = getChilds(dbo.CustomerCategories, CategoryIDsForUser).Select(i => i.ID).ToList();

            }
            var CustomerContract =
                dbo.CustomerContracts.Where(i => i.Customer.CustomerCategoryCustomers
                                                    .Any(c => catePermissIds.Contains(c.CustomerCategoryID))
                                            )
                                        .OrderByDescending(i => i.CreateAt)
                                        .Filter(filter)
                                        .ToList();
            List<CustomerContractItem> lst = new List<CustomerContractItem>();
            if (CustomerContract.Count > 0)
            {
                foreach (var item in CustomerContract)
                {
                    lst.Add(new CustomerContractItem()
                                         {
                                             ID = item.ID,
                                             Code = item.Code,
                                             CustomerID = item.CustomerID,
                                             CustomerName = item.Customer.Name,
                                             Total = item.Total,
                                             Name = item.Name,
                                             FinishDate = item.FinishDate,
                                             IsFinish = item.IsFinish,
                                             IsPause = item.IsPause,
                                             IsSuccess = item.IsSuccess,
                                             IsSend = item.IsSend,
                                             IsSignReview = item.IsSignReview,
                                             IsSignCompany = item.IsSignCompany,
                                             IsSignCustomer = item.IsSignCustomer,
                                             IsAccept = item.IsAccept,
                                             IsApproval = item.IsApproval,
                                             RateFinish = item.IsFinish ? 100 : GetRateFinishContract(item.ID),
                                             IsCancel = item.IsCancel,
                                             IsEdit = item.IsEdit,
                                             IsStart = item.IsStart,
                                             CreateAt = item.CreateAt,
                                             AttachmentFileIDs = dbo.CustomerContractAttachmentFiles.Where(t => t.CustomerContractID == item.ID).Select(x => x.AttachmentFileID).ToList()
                                         });
                }
            }
            return lst;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<CustomerContractItem> GetForAccountting(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerContractDA.Repository;
            var lstContract = CustomerContractDA.GetQuery(i => i.IsSend)
                                        .Select(item => new CustomerContractItem()
                                        {
                                            ID = item.ID,
                                            Code = item.Code,
                                            AccountingPaymentTotal = item.AccountingPayments.SelectMany(i => i.AccountingPaymentPlans)
                                                                                .Where(i => i.TimePlan == null || i.TimePlan < DateTime.Now)
                                                                                .Select(i => i.ValuePlan == null ? 0 : i.ValuePlan)
                                                                                .Sum(i => i)
                                                                    - item.AccountingPayments.SelectMany(i => i.AccountingPaymentPlans)
                                                                                .Select(i => i.ValueReal == null ? 0 : i.ValueReal)
                                                                                .Sum(i => i)
                                                                                ,
                                            MaxTime = item.AccountingPayments.SelectMany(i => i.AccountingPaymentPlans)
                                                            .Where(i => i.TimePlan == null || i.TimePlan < DateTime.Now)
                                                            .Max(i => i.TimePlan),
                                            CustomerID = item.CustomerID,
                                            CustomerName = item.Customer.Name,
                                            Total = item.Total,
                                            Name = item.Name,
                                            FinishDate = item.FinishDate,
                                            IsFinish = item.IsFinish,
                                            IsPause = item.IsPause,
                                            IsSuccess = item.IsSuccess,
                                            IsSend = item.IsSend,
                                            IsSignReview = item.IsSignReview,
                                            IsSignCompany = item.IsSignCompany,
                                            IsSignCustomer = item.IsSignCustomer,
                                            IsAccept = item.IsAccept,
                                            IsApproval = item.IsApproval,
                                            IsCancel = item.IsCancel,
                                            IsEdit = item.IsEdit,
                                            IsStart = item.IsStart,
                                            CreateAt = item.CreateAt,
                                            AttachmentFileIDs = dbo.CustomerContractAttachmentFiles.Where(t => t.CustomerContractID == item.ID).Select(x => x.AttachmentFileID).ToList()
                                        })
                                        .OrderBy(i => i.CreateAt)
                                        .Page(page, pageSize, out totalCount)
                                        .ToList();
            var lstAccountingDebtType = new AccountingDebtTypeSV().GetAll().ToList();
            var result = new List<CustomerContractItem>();
            foreach (var contract in lstContract)
            {
                if (!contract.IsCancel)
                {
                    if (contract.AccountingPaymentTotal > 0)
                    {
                        var overtime = (DateTime.Now.Date - contract.MaxTime.GetValueOrDefault(DateTime.Now).Date).Days;
                        var deptType = lstAccountingDebtType.Where(i => i.Day <= overtime && (decimal)contract.AccountingPaymentTotal >= i.Value)
                                                                  .OrderByDescending(i => i.Day)
                                                                  .OrderByDescending(i => i.Value).FirstOrDefault();
                        contract.DeptType = deptType != null ? deptType.Name : string.Empty;
                    }
                    else
                    {
                        contract.DeptType = string.Empty;
                    }
                }
                result.Add(contract);
            }
            return result;
        }
        /// <summary>
        /// Lấy hợp đồng theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerContractItem GetById(int Id)
        {
            var dbo = CustomerContractDA.Repository;
            var CustomerContract = CustomerContractDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerContractItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            CustomerName = item.Customer.Name,
                            Code = item.Code,
                            Name = item.Name,
                            Total = item.Total,
                            StatTime = item.StatTime,
                            EndTime = item.EndTime,
                            FinishDate = item.FinishDate,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            IsSuccess = item.IsSuccess,
                            IsSend = item.IsSend,
                            IsSignReview = item.IsSignReview,
                            IsSignCompany = item.IsSignCompany,
                            Note = item.Note,
                            Content = item.Content,
                            IsSignCustomer = item.IsSignCustomer,
                            SignCustomerAt = item.SignCustomerAt,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalNote = item.ApprovalNote,
                            ApprovalBy = item.ApprovalBy,
                            CompleteDate = item.CompleteDate,
                            IsCancel = item.IsCancel,
                            CancelNote = item.CancelNote,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            if (CustomerContract != null)
            {
                if (CustomerContract.ApprovalBy != null)
                {
                    CustomerContract.Approval = new HumanEmployeeViewItem()
                            {
                                ID = (int)CustomerContract.ApprovalBy,
                                Name = dbo.HumanEmployees.Where(m => m.ID == CustomerContract.ApprovalBy).Select(i => i.Name).FirstOrDefault(),
                                Role = dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == CustomerContract.ApprovalBy).Select(i => i.HumanRole.Name).FirstOrDefault(),
                                Department = dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == CustomerContract.ApprovalBy).Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault()
                            };
                }
                CustomerContract.TotalOrder = dbo.CustomerOrders.Where(i => i.CustomerContractID == CustomerContract.ID)
                                                    .Select(item => new { cost = item.Quantity * item.Service.Cost })
                                                    .Sum(i => i.cost);
                CustomerContract.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.CustomerContractAttachmentFiles.Where(i => i.CustomerContractID == Id)
                        .Select(i => i.AttachmentFileID).ToList()
                };

            }
            return CustomerContract;
        }
        /// <summary>
        /// Lấy danh sách đơn hàng của hợp đồng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="customerId"></param>
        /// <param name="contractId"></param>
        /// <returns></returns>
        public List<CustomerOrderSelected> GetOrder(int page, int pageSize, out int totalCount, int customerId, int contractId)
        {
            return new CustomerOrderSV().GetByContract(page, pageSize, out  totalCount, contractId);
        }
        /// <summary>
        /// Cập nhật hợp đồng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerContractItem item, int userID)
        {
            var CustomerContract = CustomerContractDA.GetById(item.ID);
            CustomerContract.CustomerID = item.CustomerID;
            CustomerContract.Name = item.Name;
            CustomerContract.Code = item.Code;
            CustomerContract.Content = item.Content;
            CustomerContract.Total = item.Total;
            CustomerContract.Note = item.Note;
            CustomerContract.StatTime = item.StatTime;
            CustomerContract.EndTime = item.EndTime;
            CustomerContract.FinishDate = (DateTime)item.EndTime;
            CustomerContract.Total = item.Total;
            CustomerContract.Content = item.Content;
            CustomerContract.IsEdit = item.IsEdit;
            CustomerContract.IsSignCustomer = item.IsSignCustomer;
            CustomerContract.SignCustomerAt = item.SignCustomerAt;
            CustomerContract.UpdateAt = DateTime.Now;
            CustomerContract.UpdateBy = userID;
            CustomerContractDA.Save();
            if (item.AttachmentFiles != null && item.AttachmentFiles.FileAttachments.Count > 0)
            {
                var Ids = new FileSV().InsertRange(item.AttachmentFiles.FileAttachments, userID);
                new CustomerContractAttachmentFileSV().InsertRange(Ids, CustomerContract.ID, userID);
            }
            if (item.AttachmentFiles != null && item.AttachmentFiles.FileRemoves.Count > 0)
            {
                new CustomerContractAttachmentFileSV().DeleteRange(item.AttachmentFiles.FileRemoves);
                new FileSV().DeleteRange(item.AttachmentFiles.FileRemoves);
            }
        }
        /// <summary>
        /// Thêm mới hợp đồng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerContractItem item, int userID)
        {
            var CustomerContract = new CustomerContract()
            {
                CustomerID = item.CustomerID,
                Code = item.Code,
                Name = item.Name,
                StatTime = item.StatTime,
                EndTime = item.EndTime,
                FinishDate = (DateTime)item.EndTime,
                Total = item.Total,
                IsEdit = item.IsEdit,
                IsSignCustomer = item.IsSignCustomer,
                SignCustomerAt = item.SignCustomerAt,
                Note = item.Note,
                Content = item.Content,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };

            CustomerContractDA.Insert(CustomerContract);
            CustomerContractDA.Save();
            var Ids = new FileSV().InsertRange(item.AttachmentFiles.FileAttachments, userID);
            new CustomerContractAttachmentFileSV().InsertRange(Ids, CustomerContract.ID, userID);

        }
        /// <summary>
        /// Xóa hợp đồng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerContractDA.Delete(id);
            CustomerContractDA.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isPause"></param>
        /// <param name="userID"></param>
        public void Pause(int id, bool isPause, int userID)
        {
            var contract = CustomerContractDA.GetById(id);
            contract.IsPause = isPause;
            if(isPause)
            contract.IsFinish = false;
            contract.UpdateAt = DateTime.Now;
            contract.UpdateBy = userID;
            CustomerContractDA.Update(contract);
            CustomerContractDA.Save();
        }
        /// <summary>
        /// Gửi hợp đồng cho bộ phận kế toán
        /// </summary>
        /// <param name="data"></param>
        /// <param name="p"></param>
        public void SendAccounting(CustomerContractItem data, int userId)
        {
            var contract = CustomerContractDA.GetById(data.ID);
            contract.IsSend = true;
            CustomerContractDA.Save();
        }

        /// <summary>
        /// Gửi duyệt
        /// </summary>
        /// <param name="contractItem"></param>
        public void SendApproval(CustomerContractItem contractItem)
        {
            var contract = CustomerContractDA.GetById(contractItem.ID);
            if (contractItem.IsSendApproval)
            {
                contract.IsEdit = false;
                contract.IsApproval = false;
            }
            contract.ApprovalBy = contractItem.ApprovalBy;
            CustomerContractDA.Save();
        }
        /// <summary>
        /// Phê duyệt
        /// </summary>
        /// <param name="contractItem"></param>
        public void Approve(CustomerContractItem contractItem)
        {
            var contract = CustomerContractDA.GetById(contractItem.ID);
            contract.IsApproval = true;
            contract.IsEdit = contractItem.IsEdit;
            contract.ApprovalAt = contractItem.ApprovalAt;
            contract.IsAccept = contractItem.IsAccept;
            //contract.IsPause = contractItem.IsPause;
            //contract.IsFinish = contractItem.IsFinish;
            contract.ApprovalNote = contractItem.ApprovalNote;
            //contract.IsSignCompany = contractItem.IsSignCompany;
            //if (contractItem.IsSignCompany == true)
            //    contract.IsSignReview = true;
            CustomerContractDA.Save();
        }
        public void SignCompany(CustomerContractItem contractItem)
        {
            var contract = CustomerContractDA.GetById(contractItem.ID);
            contract.IsSignReview = true;
            contract.IsSignCompany = contractItem.IsSignCompany;
            CustomerContractDA.Save();
        }

        public List<AccountingPaymentItem> GetAccountingPayment(int page, int pageSize, out int totalCount, int contractId)
        {
            return new AccountingPaymentSV().GetByContract(page, pageSize, out totalCount, contractId);
        }
        public List<AccountingPaymentItem> GetAccountingPaymentForAccounting(int page, int pageSize, out int totalCount, int contractId)
        {
            return new AccountingPaymentSV().GetByContractForAccounting(page, pageSize, out totalCount, contractId);
        }
        public List<AccountingPaymentItem> GetAccountingPaymentForCustomer(int page, int pageSize, out int totalCount, int contractId)
        {
            return new AccountingPaymentSV().GetByContractForCustomer(page, pageSize, out totalCount, contractId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AccountingPaymentItem GetAccountingPaymentById(int id)
        {
            return new AccountingPaymentSV().GetByID(id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userID"></param>
        public void AccountingPaymentInsert(AccountingPaymentItem data, int userID)
        {
            data.IsCustomer = true;
            new AccountingPaymentSV().Insert(data, userID);
        }
        /// <summary>
        /// Cập nhật đề xuất thanh toán
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userID"></param>
        public void AccountingPaymentUpdate(AccountingPaymentItem data, int userID)
        {
            data.IsCustomer = true;
            new AccountingPaymentSV().Update(data, userID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userID"></param>
        public void AccountingPaymentDelete(int accountingPaymentId)
        {
            new AccountingPaymentSV().Delete(accountingPaymentId);
        }

        public decimal? GetTotalByID(int customerContractId)
        {
            return CustomerContractDA.GetById(customerContractId).Total;
        }
        public void CancelContract(CustomerContractItem data, int userID)
        {
            var contract = CustomerContractDA.GetById(data.ID);
            contract.IsCancel = data.IsCancel;
            contract.CancelNote = data.CancelNote;
            contract.UpdateAt = DateTime.Now;
            contract.UpdateBy = userID;
            CustomerContractDA.Update(contract);
            CustomerContractDA.Save();
        }
        public void FinishContract(CustomerContractItem data, int userID)
        {
            var contract = CustomerContractDA.GetById(data.ID);
            contract.IsFinish = data.IsFinish;
            contract.CompleteDate = data.CompleteDate;
            contract.UpdateAt = DateTime.Now;
            contract.UpdateBy = userID;
            CustomerContractDA.Update(contract);
            CustomerContractDA.Save();
        }
        public void EndContract(CustomerContractItem data, int userID)
        {
            var contract = CustomerContractDA.GetById(data.ID);
            contract.IsSuccess = data.IsSuccess;
            contract.UpdateAt = DateTime.Now;
            contract.UpdateBy = userID;
            CustomerContractDA.Update(contract);
            CustomerContractDA.Save();
        }
    }
}
