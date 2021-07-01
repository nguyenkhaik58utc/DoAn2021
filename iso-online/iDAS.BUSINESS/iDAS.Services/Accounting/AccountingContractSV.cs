using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Services;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class AccountingContractSV
    {
        AccountingPaymentSV AccountingPaymentSV = new AccountingPaymentSV();
        public List<CustomerContractItem> GetContract(int page, int pageSize, out int totalCount)
        {
            var customerContractSV = new CustomerContractSV();
            return customerContractSV.GetForAccountting(page, pageSize, out  totalCount);
        }

        public List<AccountingPaymentItem> GetAccountingPayment(int page, int pageSize, out int totalCount, int contractId)
        {
            return AccountingPaymentSV.GetByContract(page, pageSize, out totalCount, contractId);
        }

        public List<AccountingPaymentItem> GetAccountingPaymentForAccounting(int page, int pageSize, out int totalCount, int contractId)
        {
            return AccountingPaymentSV.GetByContractForAccounting(page, pageSize, out totalCount, contractId);
        }
        public List<AccountingPaymentPlanItem> GetPlan(int page, int pageSize, out int totalCount, int contractId)
        {
            return new AccountingPaymentPlanSV().GetByContract(page, pageSize, out  totalCount, contractId);
        }
        public AccountingPaymentItem GetAccountingPaymentById(int id)
        {
            return AccountingPaymentSV.GetByID(id);
        }

        public void AccountingPaymentUpdate(AccountingPaymentItem data, int userID)
        {
            data.IsCustomer = false;
            AccountingPaymentSV.Update(data, userID);
        }

        public void AccountingPaymentInsert(AccountingPaymentItem data, int userID)
        {
            data.IsCustomer = false;
            AccountingPaymentSV.Insert(data, userID);
        }

        public void AccountingPaymentDelete(int ID)
        {
            AccountingPaymentSV.Delete(ID);
        }

        public CustomerContractItem GetContract(int ID)
        {
            var result = new CustomerContractSV().GetById(ID);
            return result;
        }

        public void Approve(CustomerContractItem data)
        {
            new CustomerContractSV().Approve(data);
        }

        public void SendApprove(CustomerContractItem data)
        {
            new CustomerContractSV().SendApproval(data);
        }

        public AccountingPaymentPlanItem GetPlan(int id)
        {
            return new AccountingPaymentPlanSV().GetByID(id);
        }

        public void UpdatePlan(AccountingPaymentPlanItem item, int userID)
        {
            var AccountingPaymentPlanDA = new AccountingPaymentPlanDA();
            var dbo = AccountingPaymentPlanDA.Repository;

            #region Cập nhật kế hoạch
            if (item.ID != 0)
            {
                AccountingPaymentPlan obj = AccountingPaymentPlanDA.GetById(item.ID);
                obj.AccountingPaymentID = item.AccountingPaymentID;
                obj.QualityPlanID = item.QualityPlanID;
                obj.RatePlan = item.RatePlan;
                obj.ValuePlan = item.ValuePlan;
                obj.TimePlan = item.TimePlan;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = userID;
                var pI = dbo.QualityPlans.FirstOrDefault(i => i.ID == item.QualityPlanID);
                pI.Name = item.Name;
                pI.QualityTargetID = item.TargetID;
                pI.DepartmentID = item.Department.ID;
                pI.ParentID = item.ParentID;
                pI.Type = item.Type;
                pI.Content = item.Content;
                pI.Cost = item.Cost;
                pI.StartTime = item.StartTime;
                pI.EndTime = item.EndTime;
                pI.IsApproval = item.IsApproval;
                pI.IsEdit = item.IsEdit;
                pI.IsAccept = item.IsAccept;
                pI.ApprovalAt = item.ApprovalAt;
                pI.ApprovalBy = item.ApprovalBy;
                pI.Note = item.ApprovalNote;
                pI.UpdateAt = DateTime.Now;
                pI.UpdateBy = userID;
                AccountingPaymentPlanDA.Update(obj);
                AccountingPaymentPlanDA.Save();
            }
            #endregion

            #region Thêm mới kế hoạch
            else
            {
                var plan = new QualityPlan()
                {
                    Name = item.Name,
                    QualityTargetID = item.TargetID,
                    DepartmentID = item.Department.ID,
                    ParentID = item.ParentID,
                    Type = item.Type,
                    Note = item.ApprovalNote,
                    Cost = item.Cost,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Content = item.Content,
                    // Thêm mới mặc đinh là chưa xóa và cho phép sửa
                    IsEdit = item.IsEdit,
                    // Nội dung phê duyệt
                    IsAccept = item.IsAccept,
                    IsApproval = item.IsApproval,
                    ApprovalBy = item.ApprovalBy,
                    // Thông tin hệ thống
                    CreateAt = DateTime.Now,
                    CreateBy = userID,

                };
                dbo.QualityPlans.Add(plan);
                int QualityPlanID = plan.ID;
                AccountingPaymentPlan obj = new AccountingPaymentPlan();
                obj.AccountingPaymentID = item.AccountingPaymentID;
                obj.QualityPlanID = QualityPlanID;
                obj.RatePlan = item.RatePlan;
                obj.ValuePlan = item.ValuePlan;
                obj.TimePlan = item.TimePlan;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = userID;
                AccountingPaymentPlanDA.Insert(obj);
                AccountingPaymentPlanDA.Save();
            }
            #endregion
        }
        public void RealPayment(AccountingPaymentPlanItem item, int userID)
        {
            var AccountingPaymentPlanDA = new AccountingPaymentPlanDA();
            AccountingPaymentPlan obj = AccountingPaymentPlanDA.GetById(item.ID);
            obj.RateReal = item.RateReal;
            obj.ValueReal = item.ValueReal;
            obj.TimeReal = item.TimeReal;
            AccountingPaymentPlanDA.Save();
        }
        public void Approve(AccountingPaymentPlanItem data, int userId)
        {
            var PlanDA = new QualityPlanDA();
            var pl = PlanDA.GetById(data.QualityPlanID);
            pl.IsApproval = true;
            pl.IsEdit = data.IsEdit;
            pl.ApprovalAt = data.ApprovalAt;
            pl.IsAccept = data.IsAccept;
            pl.Note = data.ApprovalNote;
            PlanDA.Save();
        }

        public void DeleteAccountingPlan(int id)
        {
            var AccountingPaymentPlanDA = new AccountingPaymentPlanDA();
            var PlanDA = new QualityPlanDA();
            var accountingPaymentPlan = AccountingPaymentPlanDA.GetById(id);
            PlanDA.Delete(accountingPaymentPlan.QualityPlanID);
            AccountingPaymentPlanDA.Delete(id);
            AccountingPaymentPlanDA.Save();
        }

        public decimal? GetTotalByID(int customerContractId)
        {
            return new CustomerContractSV().GetTotalByID(customerContractId);
        }

        public void EndContract(CustomerContractItem data, int userId)
        {
            new CustomerContractSV().EndContract(data, userId);
        }
    }
}
