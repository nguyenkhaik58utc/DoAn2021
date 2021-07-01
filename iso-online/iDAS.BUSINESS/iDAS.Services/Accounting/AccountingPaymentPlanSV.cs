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
    public class AccountingPaymentPlanSV
    {
        AccountingPaymentPlanDA AccountingPaymentPlanDA = new AccountingPaymentPlanDA();

        public List<AccountingPaymentPlanItem> GetAll(int page, int pageSize, out int total)
        {
            var accountingPayment = AccountingPaymentPlanDA.GetQuery()
                           .Select(item => new AccountingPaymentPlanItem
                           {
                               ID = item.ID,
                               AccountingPaymentID = item.AccountingPaymentID,
                               QualityPlanID = item.QualityPlanID,
                               RatePlan = item.RatePlan,
                               ValuePlan = item.ValuePlan,
                               TimePlan = item.TimePlan,
                               RateReal = item.RateReal,
                               ValueReal = item.ValueReal,
                               TimeReal = item.TimeReal,
                               CreateAt = item.CreateAt,
                           }
                           )
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return accountingPayment;
        }
        public List<AccountingPaymentPlanItem> GetByContract(int page, int pageSize, out int total, int contractId)
        {
            var dbo = AccountingPaymentPlanDA.Repository;
            var accountingPayment = dbo.AccountingPayments.Where(i => i.CustomerContractID == contractId)
                            .SelectMany(item => item.AccountingPaymentPlans)
                           .Select(item => new AccountingPaymentPlanItem
                           {
                               ID = item.ID,
                               Name = item.QualityPlan.Name,
                               AccountingPaymentID = item.AccountingPaymentID,
                               QualityPlanID = item.QualityPlanID,
                               TotalContract = item.AccountingPayment.CustomerContract.Total,
                               AccountingPayment = item.AccountingPayment.Content,
                               FinishDate = item.AccountingPayment.CustomerContract.FinishDate,
                               TargetID = item.QualityPlan.QualityTargetID,
                               //Department = new HumanDepartmentViewItem()
                               //{
                               //    ID = item.QualityPlan.DepartmentID,
                               //    Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.QualityPlan.DepartmentID).Name
                               //},
                               ValuePlan = item.ValuePlan,
                               ValueReal = item.ValueReal,
                               RatePlan = item.RatePlan,
                               RateReal = item.RateReal,
                               ParentID = item.QualityPlan.ParentID,
                               Type = item.QualityPlan.Type,
                               Cost = item.QualityPlan.Cost,
                               StartTime = item.QualityPlan.StartTime,
                               EndTime = item.QualityPlan.EndTime,
                               TimePlan = item.TimePlan,
                               Content = item.QualityPlan.Content,
                               IsEdit = item.QualityPlan.IsEdit,
                               IsApproval = item.QualityPlan.IsApproval,
                               IsAccept = item.QualityPlan.IsAccept,
                               CreateAt = item.CreateAt
                           }
                           )
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return accountingPayment;
        }
        public AccountingPaymentPlanItem GetByID(int id)
        {
            var result = AccountingPaymentPlanDA.GetQuery(i => i.ID == id)
            .Select(item => new AccountingPaymentPlanItem
            {
                ID = item.ID,
                AccountingPaymentID = item.AccountingPaymentID,
                TotalContract = item.AccountingPayment.CustomerContract.Total,
                QualityPlanID = item.QualityPlanID,
                RatePlan = item.RatePlan,
                ValuePlan = item.ValuePlan,
                TimePlan = item.TimePlan,
                RateReal = item.RateReal,
                ValueReal = item.ValueReal,
                TimeReal = item.TimeReal,
            }).First();
            if (result != null && result.QualityPlanID != 0)
            {
                var PlanItem = new QualityPlanSV().GetById(result.QualityPlanID);
                var dbo = AccountingPaymentPlanDA.Repository;
                result.Name = PlanItem.Name;
                result.TargetID = PlanItem.TargetID;
                result.Department = PlanItem.Department;
                result.ParentID = PlanItem.ParentID;
                result.ParentName = dbo.QualityPlans.FirstOrDefault(p => p.ID == PlanItem.ParentID) != null ?
                                    dbo.QualityPlans.FirstOrDefault(p => p.ID == PlanItem.ParentID).Name : string.Empty;
                result.Type = PlanItem.Type;
                result.ApprovalNote = PlanItem.ApprovalNote;
                result.Cost = PlanItem.Cost;
                result.StartTime = PlanItem.StartTime;
                result.EndTime = PlanItem.EndTime;
                result.Content = PlanItem.Content;
                result.IsApproval = PlanItem.IsApproval;
                result.ApprovalAt = PlanItem.ApprovalAt;
                result.ApprovalBy = PlanItem.ApprovalBy;
                result.IsAccept = PlanItem.IsAccept;
                result.IsEdit = PlanItem.IsEdit;
                result.IsDelete = PlanItem.IsDelete;
                result.Approval = new HumanEmployeeViewItem()
                {
                    ID = PlanItem.ApprovalBy != null ? (int)PlanItem.ApprovalBy : 0,
                    Name = PlanItem.ApprovalBy == null ?string.Empty: dbo.HumanEmployees.FirstOrDefault(m => m.ID == PlanItem.ApprovalBy).Name,
                    Role = PlanItem.ApprovalBy == null ? string.Empty : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == PlanItem.ApprovalBy).HumanRole.Name,
                    Department = PlanItem.ApprovalBy == null ? string.Empty : dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == PlanItem.ApprovalBy).HumanRole.HumanDepartment.Name,
                };
            }
            return result;
        }
        public void Insert(AccountingPaymentPlanItem item)
        {
            AccountingPaymentPlan obj = new AccountingPaymentPlan();
            obj.AccountingPaymentID = item.AccountingPaymentID;
            obj.QualityPlanID = item.QualityPlanID;
            obj.RatePlan = item.RatePlan;
            obj.ValuePlan = item.ValuePlan;
            obj.TimePlan = item.TimePlan;
            //obj.RateReal = item.RateReal;
            //obj.ValueReal = item.ValueReal;
            //obj.TimeReal = item.TimeReal;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = item.CreateBy;
            AccountingPaymentPlanDA.Insert(obj);
            AccountingPaymentPlanDA.Save();
        }

        public void Update(AccountingPaymentPlanItem item, int userId)
        {
            AccountingPaymentPlan obj = AccountingPaymentPlanDA.GetById(item.ID);
            obj.AccountingPaymentID = item.AccountingPaymentID;
            obj.QualityPlanID = item.QualityPlanID;
            obj.RatePlan = item.RatePlan;
            obj.ValuePlan = item.ValuePlan;
            obj.TimePlan = item.TimePlan;
            //obj.RateReal = item.RateReal;
            //obj.ValueReal = item.ValueReal;
            //obj.TimeReal = item.TimeReal;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            AccountingPaymentPlanDA.Update(obj);
            AccountingPaymentPlanDA.Save();
        }

        public void Delete(int id)
        {
            var result = AccountingPaymentPlanDA.GetById(id);
            AccountingPaymentPlanDA.Delete(result);
            AccountingPaymentPlanDA.Save();
        }
    }
}
