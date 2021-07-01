using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class ServiceContractCommandSV
    {
        //   private CustomerContactHistoryService contactSV = new CustomerContactHistoryService();
        private ServiceCommandDA commandServiceDA = new ServiceCommandDA();
        private ServiceCommandContractDA commandContractDA = new ServiceCommandContractDA();
        private CustomerContractDA customerContractDA = new CustomerContractDA();
        public ServiceCommandContract GetByCommandIDAndContractID(int commandId, int contractId)
        {
            var checks = commandContractDA.GetQuery(t => t.ServiceCommandID == commandId && t.CustomerContractID == contractId).FirstOrDefault();
            return checks;
        }

        public int?[] GetListIDByCommandID(int commandId)
        {
            try
            {
                var modules = commandContractDA.GetQuery()
                                    .Where(a => a.ServiceCommandID == commandId)
                                    .Select(a => a.CustomerContractID).ToArray();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool SetIsSelected(int commandId, int contractId)
        {
            if (GetListIDByCommandID(commandId).Contains(contractId))
            {
                return true;
            }
            return false;
        }
        public List<CustomerContractItem> GetAllForCommand(int page, int pageSize, out int total, int commandId)
        {
            var dbo = customerContractDA.Repository;
            List<CustomerContractItem> lst = new List<CustomerContractItem>();
            var createdCommand = dbo.ServiceCommandContracts.Select(t=>t.CustomerContractID).ToArray();
            var contracts = customerContractDA
                .GetQuery(t => t.IsSignCompany && t.IsSignCustomer && !t.IsFinish && !t.IsCancel && !t.IsSuccess && !t.IsPause && !t.IsEdit)
                .Where(t=>!createdCommand.Contains(t.ID))
                .OrderByDescending(t => t.CreateAt)
                .Page(page, pageSize, out total)
                .ToList();
            if (contracts.Count > 0)
            {
                foreach (var item in contracts)
                {
                    lst.Add(new CustomerContractItem
                    {
                        ID = item.ID,
                        Code = item.Code,
                        CustomerID = item.CustomerID,
                        CustomerName = item.Customer.Name,
                        Name = item.Name,
                        IsSelected = SetIsSelected(commandId, item.ID)
                    });
                }
            }
            return lst;
        }
        public List<CustomerContractItem> GetAllForCommandEdit(int page, int pageSize, out int total, int commandId)
        {
            var dbo = customerContractDA.Repository;
            List<CustomerContractItem> lst = new List<CustomerContractItem>();
            var lstCommandExit = new List<int>();
            var sx= dbo.ServiceCommands.FirstOrDefault(t=>t.ID==commandId).ServiceCommandContracts.Select(t=>t.CustomerContractID).ToArray();
            var createdCommand = dbo.ServiceCommandContracts.Where(t=>!sx.Contains(t.CustomerContractID)).Select(t => t.CustomerContractID).ToArray();
            var contracts = customerContractDA
                .GetQuery(t => t.IsSignCompany && t.IsSignCustomer && !t.IsFinish)
                .Where(t => !createdCommand.Contains(t.ID))
                .OrderByDescending(t => t.CreateAt)
                .Page(page, pageSize, out total)
                .ToList();
            if (contracts.Count > 0)
            {
                foreach (var item in contracts)
                {
                    lst.Add(new CustomerContractItem
                    {
                        ID = item.ID,
                        Code = item.Code,
                        CustomerID = item.CustomerID,
                        CustomerName = item.Customer.Name,
                        Name = item.Name,
                        IsSelected = SetIsSelected(commandId, item.ID)
                    });
                }
            }
            return lst;
        }
        public List<CustomerContractItem> GetAllForCommandShow(int page, int pageSize, out int total, int commandId)
        {
            List<CustomerContractItem> lst = new List<CustomerContractItem>();
            int?[] s = GetListIDByCommandID(commandId);
            var isos = customerContractDA.GetQuery(t => s.Contains(t.ID)).OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (isos.Count > 0)
            {
                foreach (var item in isos)
                {
                    lst.Add(new CustomerContractItem
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
                        IsSelected = SetIsSelected(commandId, item.ID)
                    });
                }
            }
            return lst;
        }
        public int[] GetListID()
        {
            try
            {
                var modules = commandContractDA.GetQuery()
                                    .Select(a => a.ID).ToArray();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int[] GetListServiceID(int contractId)
        {
            try
            {
                var dbo = commandContractDA.Repository;
                var modules = dbo.CustomerOrders.Where(t => t.CustomerContractID == contractId).Select(t => t.ServiceID).ToArray();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int[] GetListStageID(int servicePlanId, int customerId)
        {
            try
            {
                var dbo = commandContractDA.Repository;
                var modules = dbo.ServicePlanStages.Where(t => t.CustomerID == customerId && t.ServicePlanID == servicePlanId).Select(t => t.ServiceStageID).ToArray();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ServiceStageItem> GetStageByContractID(int contractId, int customerId, int servicePlanId)
        {
            int[] serviceIds = GetListServiceID(contractId);
            var dbo = commandContractDA.Repository;
            List<ServiceStageItem> lst = new List<ServiceStageItem>();
            var stage = dbo.ServiceStages.Where(t => serviceIds.Contains(t.ServiceID) && t.IsUse).OrderBy(t => t.Order).ToList();
            if (stage.Count > 0)
            {
                foreach (var item in stage)
                {
                    lst.Add(new ServiceStageItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ServiceName = "Dịch vụ: " + item.Service.Name,
                        IsSelected = dbo.ServicePlanStages.FirstOrDefault(t => t.CustomerID == customerId && t.ServicePlanID == servicePlanId && t.ServiceStageID == item.ID) != null ? true : false
                    });
                }
            }
            return lst;
        }
        public List<ServiceStageItem> GetStageByContractIDForDetail(int contractId, int customerId, int servicePlanId)
        {
            int[] stageIds = GetListStageID(servicePlanId, customerId);
            var dbo = commandContractDA.Repository;
            List<ServiceStageItem> lst = new List<ServiceStageItem>();
            var stage = dbo.ServiceStages.Where(t => stageIds.Contains(t.ID)).OrderByDescending(t => t.CreateAt).ToList();
            if (stage.Count > 0)
            {
                foreach (var item in stage)
                {
                    lst.Add(new ServiceStageItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ServiceName = item.Service.Name,
                        IsSelected = dbo.ServicePlanStages.FirstOrDefault(t => t.CustomerID == customerId && t.ServicePlanID == servicePlanId && t.ServiceStageID == item.ID) != null ? true : false
                    });
                }
            }

            return lst;
        }
        public List<ServiceCommandContractItem> GetByCommandID(int commandId)
        {
            List<ServiceCommandContractItem> lst = new List<ServiceCommandContractItem>();
            var dbo = commandContractDA.Repository;
            var services = commandContractDA.GetQuery(t => t.ServiceCommandID == commandId).OrderByDescending(t => t.CreateAt).ToList();
            if (services.Count > 0)
            {
                foreach (var item in services)
                {
                    lst.Add(new ServiceCommandContractItem
                    {
                        ID = item.ID,
                        ContractID = (int)item.CustomerContractID,
                        ContractName = item.CustomerContract.Name,
                        CommandName = "Lệnh: " + item.ServiceCommand.Name,
                        CustomerID = item.CustomerContract.CustomerID,
                        CustomerName = item.CustomerContract.Customer.Name
                    });
                }
            }
            return lst;
        }
        public List<ServiceCommandContractItem> GetByID(int id)
        {
            List<ServiceCommandContractItem> lst = new List<ServiceCommandContractItem>();
            var dbo = commandContractDA.Repository;
            var services = commandContractDA.GetQuery(t => t.ID == id).ToList();
            foreach (var item in services)
            {
                lst.Add(new ServiceCommandContractItem
                {
                    ID = item.ID,
                    ContractID = (int)item.CustomerContractID,
                    ContractName = item.CustomerContract.Name,
                    CommandName = "Lệnh: " + item.ServiceCommand.Name,
                    CustomerID = item.CustomerContract.CustomerID,
                    CustomerName = item.CustomerContract.Customer.Name,
                });
            }
            return lst;
        }
        public void Insert(ServiceCommandContract obj, int userId, int customerID)
        {
            var dbo = commandServiceDA.Repository;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            commandContractDA.Insert(obj);
            commandContractDA.Save();
            //int[] arr = contactSV.GetServiceIDByContractID((int)obj.ContractID);
            //if (arr != null)
            //{
            //    List<MnServiceCommandService> lst = new List<MnServiceCommandService>();
            //    foreach (var item in arr)
            //    {
            //        lst.Add(new MnServiceCommandService
            //        {

            //            CommandID = (int)obj.CommandID,
            //            Time = dbo.MnCustomerOrder.Where(t => t.ServiceID == (int)item && t.ContractID == obj.ContractID && t.CustomerID == customerID).FirstOrDefault()!=null?dbo.MnCustomerOrder.Where(t => t.ServiceID == (int)item && t.ContractID == obj.ContractID && t.CustomerID == customerID).FirstOrDefault().Time:DateTime.Now,
            //            ServiceID = (int)item,
            //            ContractID = obj.ContractID,
            //            CustomerID = customerID,
            //            CreateAt = DateTime.Now,
            //            CreateBy = userId
            //        });
            //    }
            //    commandServiceDA.InsertRange(lst);
            //    commandServiceDA.Save();
            //}
        }
        public void Delete(int id, int commandId)
        {
            var obj = commandContractDA.GetById(id);
            commandContractDA.Delete(obj);
            commandContractDA.Save();
        }
    }
}
