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
    public class ServicePlanSV
    {
        private ServicePlanDA planDA = new ServicePlanDA();
        private ServicePlanStageDA servicePlanStageDA = new ServicePlanStageDA();
        private ServiceContractCommandSV serviceContractCommandSV = new ServiceContractCommandSV();
        private QualityPlanSV planServiceQ = new QualityPlanSV();
        private QualityPlanDA planDAQ = new QualityPlanDA();
        private ServiceCommandServiceSV commandServiceSV = new ServiceCommandServiceSV();
        public void Delete(int id)
        {
            planServiceQ.Delete(id);
        }
        public void Update(QualityPlanItem item, int userID)
        {
            planServiceQ.Update(item, userID);
        }
        public int Insert(QualityPlanItem item)
        {
            return planServiceQ.Insert(item);
        }
        public void ApproveFromService(ServicePlanItem PlanApprovalItem)
        {
            planServiceQ.ApproveFromService(PlanApprovalItem);
        }
        public List<ServiceStageItem> GetStageByContractIDForDetail(int contractId, int customerId, int servicePlanId)
        {
            return serviceContractCommandSV.GetStageByContractIDForDetail(contractId, customerId, servicePlanId);
        }
        public List<ServiceStageItem> GetStageByContractID(int contractId, int customerId, int servicePlanId)
        {
            return serviceContractCommandSV.GetStageByContractID(contractId, customerId, servicePlanId);
        }
        public List<ServiceCommandContractItem> GetByCommandID(int commandId)
        {
            return serviceContractCommandSV.GetByCommandID(commandId);
        }
        public List<ServiceCommandContractItem> GetCommandContractByID(int id)
        {
            return serviceContractCommandSV.GetByID(id);
        }
        private int[] GetListPlanIDByServiceCommandContractID(int serviceCommandContractID)
        {
            try
            {
                int[] data = planDA.GetQuery(t => t.ServiceCommandContractID == serviceCommandContractID).Select(t => t.QualityPlanID).ToArray();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int[] GetListPlanIDByCommandID(int commandId)
        {
            try
            {
                int[] data = planDA.GetQuery(t => t.ServiceCommandContract.ServiceCommandID == commandId).Select(t => t.QualityPlanID).ToArray();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int[] GetListPlanIDByContractID(int contractId)
        {
            try
            {
                var dbo = planDA.Repository;
                int[] s = dbo.ServiceCommandContracts.Where(t => t.CustomerContractID == contractId).Select(t => t.ID).ToArray();
                int[] data = planDA.GetQuery(t => s.Contains(t.ServiceCommandContractID.Value)).Select(t => t.QualityPlanID).ToArray();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int[] GetListPlanID()
        {
            try
            {

                int[] data = planDA.GetQuery(t => t.QualityPlan.IsAccept && t.QualityPlan.IsApproval).Select(t => t.QualityPlanID).ToArray();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetPlanID(int id)
        {
            var rs = planDA.GetById(id);
            if (rs != null)
                return rs.QualityPlanID;
            return 0;

        }
        public ServicePlan GetPlanByID(int id)
        {
            var rs = planDA.GetById(id);
            return rs;

        }
        public void SendApproval(QualityPlanItem item, int userID)
        {
            planServiceQ.SendApproval(item, userID);
        }
        public List<ServicePlanItem> GetAllIsApproval(int page, int pageSize, out int totalCount, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string year)
        {
            var dbo = planDAQ.Repository;
            if (year.Trim().Equals(""))
                year = DateTime.Now.Year.ToString();
            int y1 = int.Parse(year);
            var fromDateForQr = Convert.ToDateTime(fromdate);
            var toDateQr = Convert.ToDateTime(todate).AddDays(1);
            int[] lstPlanId = GetListPlanID();
            var users = planDAQ.GetQuery(i => lstPlanId.Contains(i.ID) && i.IsAccept && i.IsApproval && i.ParentID == 0)
                        .Where(t => t.ApprovalAt.Value.Year == y1 && (t.ApprovalAt > fromDateForQr && t.ApprovalAt <= toDateQr))
                        .Select(item => new ServicePlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ServiceCommandContractID = item.ServicePlans.FirstOrDefault().ServiceCommandContractID.HasValue ? item.ServicePlans.FirstOrDefault().ServiceCommandContractID.Value : 0,
                            CommandServiceName = item.ServicePlans.FirstOrDefault().ServiceCommandContract.ServiceCommand != null ? item.ServicePlans.FirstOrDefault().ServiceCommandContract.ServiceCommand.Name : string.Empty,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public List<ServicePlanItem> GetAll(ModelFilter filter, int commandId, int focusId)
        {
            var dbo = planDAQ.Repository;
            int[] lstPlanId = GetListPlanIDByCommandID(commandId);
            IQueryable<iDAS.Base.QualityPlan> query = dbo.QualityPlans
                        .Where(t => (lstPlanId.Contains(t.ID) && t.ParentID == 0) || (commandId==0 && t.ID == focusId));
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var users = query
                        .Filter(filter)
                        .Select(item => new ServicePlanItem()
                        {
                            ID = item.ID,
                            ServicePlanID = item.ServicePlans.FirstOrDefault().ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            CommandID = item.ServicePlans.FirstOrDefault(t=>t.QualityPlanID==item.ID).ServiceCommandContract.ServiceCommandID,
                            Department = new iDAS.Items.HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            ParentID = item.ParentID,
                            Type = item.Type,
                            //ServiceName = item.ServicePlans.FirstOrDefault().ServiceCommandContract.CustomerContract.CustomerOrders.FirstOrDefault().Service.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .ToList();
            return users;
        }
        public List<ServicePlanItem> GetAllByContractID(int page, int pageSize, out int totalCount, int parentId, int contractId)
        {
            var dbo = planDAQ.Repository;
            int[] lstPlanId = GetListPlanIDByCommandID(contractId);
            List<ServicePlanItem> lst = new List<ServicePlanItem>();
            lst = planDAQ.GetQuery(i => lstPlanId.Contains(i.ID) && i.ParentID == parentId && i.IsAccept && i.IsApproval)
                          .Select(item => new ServicePlanItem()
                          {
                              ID = item.ID,
                              ServicePlanID = item.ServicePlans.FirstOrDefault().ID,
                              Name = item.Name,
                              CustomerName = "Khách hàng: " + item.ServicePlans.FirstOrDefault().ServicePlanStages.FirstOrDefault().Customer.Name + " - Dịch vụ: " + item.ServicePlans.FirstOrDefault().ServicePlanStages.FirstOrDefault().ServiceStage.Service.Name,
                              TargetID = item.QualityTargetID,
                              Department = new iDAS.Items.HumanDepartmentViewItem()
                              {
                                  ID = item.DepartmentID,
                                  Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                              },
                              ParentID = item.ParentID,
                              Type = item.Type,
                              Cost = item.Cost,
                              StartTime = item.StartTime,
                              EndTime = item.EndTime,
                              Content = item.Content,
                              IsEdit = item.IsEdit,
                              IsApproval = item.IsApproval,
                              IsAccept = item.IsAccept,
                              ApprovalAt = item.ApprovalAt,
                              ApprovalBy = item.ApprovalBy,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt,
                              CreateBy = item.CreateBy,
                              UpdateAt = item.UpdateAt,
                              UpdateBy = item.UpdateBy

                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out totalCount)
                          .ToList();

            return lst;
        }
        public List<ServicePlanItem> GetForAccounting(int page, int pageSize, out int totalCount, int contractId)
        {
            var dbo = planDAQ.Repository;
            int[] lstPlanId = GetListPlanIDByContractID(contractId);
            List<ServicePlanItem> lst = new List<ServicePlanItem>();
            var data = planDAQ.GetQuery(i => lstPlanId.Contains(i.ID) && i.ParentID != 0 && i.IsAccept && i.IsApproval)
                           .OrderByDescending(item => item.CreateAt)
                           .Page(page, pageSize, out totalCount)
                           .ToList();
            try
            {
                foreach (var item in data)
                {
                    lst.Add(new ServicePlanItem()
                    {
                        ID = item.ID,
                        ServicePlanID = item.ServicePlans.FirstOrDefault().ID,
                        Name = item.Name,
                        ParentName = dbo.QualityPlans.FirstOrDefault(t => t.ID == item.ParentID).Name,
                        TargetID = item.QualityTargetID,
                        Department = new iDAS.Items.HumanDepartmentViewItem()
                        {
                            ID = item.DepartmentID,
                            Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                        },
                        ParentID = item.ParentID,
                        Type = item.Type,
                        Cost = item.Cost,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Content = item.Content,
                        IsEdit = item.IsEdit,
                        IsApproval = item.IsApproval,
                        IsAccept = item.IsAccept,
                        ApprovalAt = item.ApprovalAt,
                        RateFinish = GetRateFinish(item.ID),
                        ApprovalBy = item.ApprovalBy,
                        IsDelete = item.IsDelete,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy

                    });

                }
            }
            catch { 
            
            }

            return lst;
        }

        public List<ServicePlanItem> GetAllByPlanParentIDIsChoice(int page, int pageSize, out int totalCount, int parentId)
        {
            var dbo = planDAQ.Repository;
            List<ServicePlanItem> lst = new List<ServicePlanItem>();
            if (parentId == 0)
            {
                parentId += -1;
            }
            var data = planDAQ.GetQuery(i => i.ParentID == parentId && i.IsAccept && i.IsApproval)
                                  .OrderByDescending(item => item.CreateAt)
                                  .Page(page, pageSize, out totalCount)
                                  .ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    var customerName = item.ServicePlans.FirstOrDefault().ServicePlanStages.FirstOrDefault() != null ? item.ServicePlans.FirstOrDefault().ServicePlanStages.FirstOrDefault().Customer.Name : string.Empty;
                    lst.Add(new ServicePlanItem()
                              {
                                  ID = item.ID,
                                  ServicePlanID = item.ServicePlans.FirstOrDefault().ID,
                                  Name = item.Name,
                                  CustomerName = customerName,
                                  //CustomerName = "Khách hàng: " + item.ServicePlans.FirstOrDefault().ServicePlanStages.FirstOrDefault()!= null ? item.ServicePlans.FirstOrDefault().ServicePlanStages.FirstOrDefault().Customer.Name : "" + " - Dịch vụ: " + item.ServicePlans.FirstOrDefault().ServicePlanStages.FirstOrDefault().ServiceStage.Service.Name,
                                  TargetID = item.QualityTargetID,
                                  Department = new iDAS.Items.HumanDepartmentViewItem()
                                  {
                                      ID = item.DepartmentID,
                                      Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                                  },
                                  ParentID = item.ParentID,
                                  Type = item.Type,
                                  Cost = item.Cost,
                                  StartTime = item.StartTime,
                                  EndTime = item.EndTime,
                                  Content = item.Content,
                                  IsEdit = item.IsEdit,
                                  IsApproval = item.IsApproval,
                                  IsAccept = item.IsAccept,
                                  ApprovalAt = item.ApprovalAt,
                                  RateFinish = GetRateFinish(item.ID),
                                  ApprovalBy = item.ApprovalBy,
                                  IsDelete = item.IsDelete,
                                  CreateAt = item.CreateAt,
                                  CreateBy = item.CreateBy,
                                  UpdateAt = item.UpdateAt,
                                  UpdateBy = item.UpdateBy
                              });
                }
            }
            return lst;
        }
        public decimal GetRateFinish(int idplanQ)
        {
            var dbo = planDAQ.Repository;
            var lstTaskID = dbo.QualityPlanTasks.Where(x => x.QualityPlanID == idplanQ).Select(x => x.TaskID).ToArray();
            if (dbo.Tasks.Where(s => lstTaskID.Contains(s.ID) && s.IsComplete).Count() != 0)
            {
                decimal rate = (System.Math.Round((decimal)dbo.Tasks.Where(s => lstTaskID.Contains(s.ID) && s.IsComplete).Count() / (decimal)dbo.Tasks.Where(s => lstTaskID.Contains(s.ID) && !s.IsPause && !s.IsNew).Count(), 2)) * 100;
                return rate;
            }
            else
            {
                return 0;
            }
        }
        public List<ServicePlanItem> GetByParentID(int page, int pageSize, out int totalCount, int serviceCommandContractID, int parentId)
        {
            int[] lstPlanId = GetListPlanIDByServiceCommandContractID(serviceCommandContractID);
            var users = planDAQ.GetQuery(i => lstPlanId.Contains(i.ID) && i.ParentID == parentId)
                        .Select(item => new ServicePlanItem()
                        {
                            ID = item.ID,
                            ServicePlanID = item.ServicePlans.FirstOrDefault().ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new iDAS.Items.HumanDepartmentViewItem() { ID = item.DepartmentID },
                            ParentID = item.ParentID,
                            Type = item.Type,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public int Insert(int planId, int serviceCommandContractID, int userId)
        {
            var obj = new ServicePlan();
            obj.QualityPlanID = planId;
            obj.ServiceCommandContractID = serviceCommandContractID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            planDA.Insert(obj);
            planDA.Save();
            return obj.ID;
        }
        public void InsertPlanDetail(int planId, int serviceCommandContractID, int userId, string strStageID, int customerId)
        {
            var obj = new ServicePlan();
            obj.QualityPlanID = planId;
            obj.ServiceCommandContractID = serviceCommandContractID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            planDA.Insert(obj);
            planDA.Save();
            if (strStageID.Trim() != "")
            {
                var contractIDs = strStageID.Split(',').Select(n => (object)int.Parse(n)).ToList();
                foreach (var item in contractIDs)
                {
                    ServicePlanStage servicePlanStage = new ServicePlanStage();
                    servicePlanStage.CreateAt = DateTime.Now;
                    servicePlanStage.CreateBy = userId;
                    servicePlanStage.CustomerID = customerId;
                    servicePlanStage.ServicePlanID = obj.ID;
                    servicePlanStage.ServiceStageID = (int)item;
                    servicePlanStageDA.Insert(servicePlanStage);
                }
                servicePlanStageDA.Save();
            }
        }
        public void Update(int planId, int serviceCommandContractID, int userId)
        {
            var id = planDAQ.GetById(planId).ServicePlans.FirstOrDefault().ID;
            var obj = planDA.GetById(id);
            obj.ServiceCommandContractID = serviceCommandContractID;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            planDA.Update(obj);
            planDA.Save();

        }
        public void UpdatePlanDetail(int planId, int serviceCommandContractID, int userId, string strStageID, int customerId)
        {
            var id = planDAQ.GetById(planId).ServicePlans.FirstOrDefault().ID;
            var obj = planDA.GetById(id);
            obj.ServiceCommandContractID = serviceCommandContractID;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            planDA.Update(obj);
            planDA.Save();
            var lst = servicePlanStageDA.GetQuery(t => t.CustomerID == customerId && t.ServicePlanID == id).ToList();
            servicePlanStageDA.DeleteRange(lst);
            if (strStageID.Trim() != "")
            {
                var contractIDs = strStageID.Split(',').Select(n => (object)int.Parse(n)).ToList();
                foreach (var item in contractIDs)
                {
                    ServicePlanStage servicePlanStage = new ServicePlanStage();
                    servicePlanStage.CreateAt = DateTime.Now;
                    servicePlanStage.CreateBy = userId;
                    servicePlanStage.CustomerID = customerId;
                    servicePlanStage.ServicePlanID = obj.ID;
                    servicePlanStage.ServiceStageID = (int)item;
                    servicePlanStageDA.Insert(servicePlanStage);
                }
                servicePlanStageDA.Save();
            }
        }
        public ServicePlanItem GetById(int Id)
        {
            var dbo = planDA.Repository;
            var PlanItem = dbo.QualityPlans.Where(i => i.ID == Id)
                        .Select(item => new ServicePlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new iDAS.Items.HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.DepartmentID).Name,
                            },
                            ServiceCommandContractID = item.ServicePlans.FirstOrDefault().ServiceCommandContractID.Value,
                            ServicePlanID = item.ServicePlans.FirstOrDefault().ID,
                            ParentID = item.ParentID,
                            ParentName = dbo.QualityPlans.FirstOrDefault(p => p.ID == item.ParentID).Name,
                            Type = item.Type,
                            ApprovalNote = item.Note,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsAccept = item.IsAccept,
                            IsEdit = item.IsEdit,
                            IsDelete = item.IsDelete,
                            Approval = new iDAS.Items.HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy != null ? (int)item.ApprovalBy : 0,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.HumanDepartment.Name,
                            },
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.HumanEmployees.FirstOrDefault(a => a.ID == dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployeeID).Name,
                            UpdateAt = item.UpdateAt,
                            UpdateByName = dbo.HumanEmployees.FirstOrDefault(a => a.ID == dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployeeID).Name,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            PlanItem.TargetName = dbo.QualityTargets.FirstOrDefault(i => i.ID == PlanItem.TargetID) != null ?
                "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).CompleteAt.ToString() + " " +
                dbo.QualityTargets.FirstOrDefault(i => i.ID == PlanItem.TargetID).Name + iDAS.Utilities.Common.GetTypeName(dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).Type) + dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).Value.ToString() + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).Unit : string.Empty;
            PlanItem.CreateByEmployeeID = PlanItem.CreateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == PlanItem.CreateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == PlanItem.CreateBy).HumanEmployeeID : null;
            PlanItem.UpdateByEmployeeID = PlanItem.UpdateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == PlanItem.UpdateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == PlanItem.UpdateBy).HumanEmployeeID : null;
            return PlanItem;
        }
    }
}
