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
    public class ServiceCommandService
    {
        private ServiceCommandDA comandDA = new ServiceCommandDA();
        private ServiceCommandContractDA serviceCommandContractDA = new ServiceCommandContractDA();
        public List<ServiceCommandItem> GetAllIsApproval(int page, int pageSize, out int total)
        {
            var audits = comandDA.GetQuery(t => t.IsAccept)
                 .Select(item => new ServiceCommandItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     EndTime = item.EndTime,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Note = item.Note,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Page(page, pageSize, out total)
                 .ToList();
            return audits;
        }
        public List<ServiceCommandItem> GetAll(ModelFilter filter, int focusId=0, int serviceId=0)
        {
            var dbo = comandDA.Repository;
            IQueryable<iDAS.Base.ServiceCommand> query = dbo.ServiceCommands;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            if (serviceId!=0)
            {
                query = query.Where(t=>t.ServiceCommandContracts.FirstOrDefault(s=>s.CustomerContract.CustomerOrders.Any(o=>o.ServiceID==serviceId))!=null);
            }
            var audits = query
                 .Filter(filter)
                 .Select(item => new ServiceCommandItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     EndTime = item.EndTime,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Note = item.Note,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .ToList();
            return audits;
        }
        public void SendApprove(ServiceCommandItem objEdit, int userId)
        {
            ServiceCommand obj = comandDA.GetById(objEdit.ID);
            obj.ApprovalBy = objEdit.Approval.ID;
            obj.IsEdit = false;
            obj.IsApproval = false;
            comandDA.Update(obj);
            comandDA.Save();

        }
        public void Approve(ServiceCommandItem objEdit, int userId)
        {
            ServiceCommand obj = comandDA.GetById(objEdit.ID);
            obj.IsAccept = objEdit.IsAccept;
            obj.ApprovalNote = objEdit.ApprovalNote;
            obj.IsApproval = true;
            obj.Note = objEdit.Note;
            obj.ApprovalAt = DateTime.Now;
            obj.IsEdit = objEdit.IsEdit;
            comandDA.Update(obj);
            comandDA.Save();

        }
        public ServiceCommandItem GetByID(int id)
        {
            ServiceCommandItem obj = new ServiceCommandItem();
            var employeeSV = new HumanEmployeeSV();
            var dbo = comandDA.Repository;
            var command = comandDA.GetById(id);
            if (command != null)
            {
                obj.ID = command.ID;
                obj.Note = command.Note;
                obj.Name = command.Name;
                obj.Note = command.Note;
                obj.ApprovalNote = command.ApprovalNote;
                obj.IsEdit = command.IsEdit;
                obj.Approval = employeeSV.GetEmployeeView(command.ApprovalBy);
                obj.Perform = employeeSV.GetEmployeeView(command.PerformBy);
                obj.EndTime = command.EndTime;
                obj.IsAccept = command.IsAccept;
                obj.CreateByEmployeeID = command.CreateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == command.CreateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == command.CreateBy).HumanEmployeeID : null;
                obj.UpdateByEmployeeID = command.UpdateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == command.UpdateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == command.UpdateBy).HumanEmployeeID : null;
                obj.IsApproval = command.IsApproval;
                obj.UpdateBy = command.UpdateBy;
                obj.CreateBy = command.CreateBy;
                obj.StartTime = command.StartTime;
                obj.IsDelete = command.IsDelete;
            }
            return obj;
        }
        public bool CheckNameExits(string name)
        {
            var rs = comandDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Insert(ServiceCommandItem objNew, string strContractID, int userId)
        {
            var obj = new ServiceCommand();
            obj.Name = objNew.Name.Trim();
            obj.StartTime = objNew.StartTime;
            obj.EndTime = objNew.EndTime;
            obj.ApprovalBy = objNew.Approval.ID;
            obj.PerformBy = objNew.Perform.ID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.IsAccept = false;
            obj.IsApproval = false;
            obj.IsDelete = false;
            obj.IsEdit = objNew.IsEdit;
            obj.Note = objNew.Note.Trim();
            comandDA.Insert(obj);
            comandDA.Save();
            if (strContractID.Trim() != "")
            {
                var contractIDs = strContractID.Split(',').Select(n => (object)int.Parse(n)).ToList();
                foreach (var item in contractIDs)
                {
                    ServiceCommandContract contract = new ServiceCommandContract();
                    contract.ServiceCommandID = obj.ID;
                    contract.CustomerContractID = (int)item;
                    contract.CreateAt = DateTime.Now;
                    contract.CreateBy = userId;
                    serviceCommandContractDA.Insert(contract);
                }
                serviceCommandContractDA.Save();
            }


        }
        public int InsertCommandService(ServiceCommandItem objNew, string strContractID, int userId)
        {
            var obj = new ServiceCommand();
            obj.Name = objNew.Name.Trim();
            obj.StartTime = objNew.StartTime;
            obj.EndTime = objNew.EndTime;
            obj.ApprovalBy = objNew.Approval.ID;
            obj.PerformBy = objNew.Perform.ID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.IsAccept = false;
            obj.IsApproval = false;
            obj.IsDelete = false;
            obj.IsEdit = objNew.IsEdit;
            obj.Note = objNew.Note.Trim();
            comandDA.Insert(obj);
            comandDA.Save();
            if (strContractID.Trim() != "")
            {
                var contractIDs = strContractID.Split(',').Select(n => (object)int.Parse(n)).ToList();
                foreach (var item in contractIDs)
                {
                    ServiceCommandContract contract = new ServiceCommandContract();
                    contract.ServiceCommandID = obj.ID;
                    contract.CustomerContractID = (int)item;
                    contract.CreateAt = DateTime.Now;
                    contract.CreateBy = userId;
                    serviceCommandContractDA.Insert(contract);
                }
                serviceCommandContractDA.Save();
            }
            return obj.ID;
        }
        public int InsertCommandApproveService(ServiceCommandItem objNew, string strContractID, int userId)
        {
            var obj = new ServiceCommand();
            obj.Name = objNew.Name.Trim();
            obj.StartTime = objNew.StartTime;
            obj.EndTime = objNew.EndTime;
            obj.ApprovalBy = objNew.Approval.ID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.IsAccept = true;
            obj.IsApproval = true;
            obj.IsDelete = false;
            obj.ApprovalAt = DateTime.Now;
            obj.PerformBy = objNew.Perform.ID;
            obj.IsEdit = objNew.IsEdit;
            obj.Note = objNew.Note.Trim();
            comandDA.Insert(obj);
            comandDA.Save();
            if (strContractID.Trim() != "")
            {
                var contractIDs = strContractID.Split(',').Select(n => (object)int.Parse(n)).ToList();
                foreach (var item in contractIDs)
                {
                    ServiceCommandContract contract = new ServiceCommandContract();
                    contract.ServiceCommandID = obj.ID;
                    contract.CustomerContractID = (int)item;
                    contract.CreateAt = DateTime.Now;
                    contract.CreateBy = userId;
                    serviceCommandContractDA.Insert(contract);
                }
                serviceCommandContractDA.Save();
            }
            return obj.ID;
        }
        public void Update(ServiceCommandItem objEdit, string strContractID, int userId)
        {
            var obj = comandDA.GetById(objEdit.ID);
            obj.Name = objEdit.Name.Trim();
            obj.StartTime = objEdit.StartTime;
            obj.EndTime = objEdit.EndTime;
            obj.IsApproval = objEdit.IsApproval;
            obj.ApprovalBy = objEdit.Approval.ID;
            obj.PerformBy = objEdit.Perform.ID;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.IsEdit = objEdit.IsEdit;
            obj.Note = objEdit.Note.Trim();
            comandDA.Update(obj);
            comandDA.Save();
            var lst = serviceCommandContractDA.GetQuery(t => t.ServiceCommandID == objEdit.ID).ToList();
            serviceCommandContractDA.DeleteRange(lst);
            if (strContractID.Trim() != "")
            {
                var contractIDs = strContractID.Split(',').Select(n => (object)int.Parse(n)).ToList();
                foreach (var item in contractIDs)
                {
                    ServiceCommandContract contract = new ServiceCommandContract();
                    contract.ServiceCommandID = obj.ID;
                    contract.CustomerContractID = (int)item;
                    contract.CreateAt = DateTime.Now;
                    contract.CreateBy = userId;
                    serviceCommandContractDA.Insert(contract);
                }
                serviceCommandContractDA.Save();
            }

        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            comandDA.DeleteRange(ids);
            comandDA.Save();
        }
        /// <summary>
        /// Dùng cho thống kê
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<ServiceCommandItem> GetTotalCommand(ModelFilter filter, int serviceId, DateTime fromDate, DateTime toDate)
        {
            var dbo = comandDA.Repository;
            var commands = dbo.CustomerOrders
                 .Where(t => t.CustomerContractID.HasValue)
                 .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                 .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                 .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                 .Where(t => t.ServiceID == serviceId)
                 .SelectMany(t => t.CustomerContract.ServiceCommandContracts)
                 .Select(t => t.ServiceCommand)
                 .Where(t => t.IsApproval)
                 .Distinct()
                 .Select(item => new ServiceCommandItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     EndTime = item.EndTime,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Note = item.Note,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Filter(filter)
                 .ToList();
            return commands;
        }
        public List<ServiceCommandItem> GetTotalCommandApproval(ModelFilter filter, int serviceId, DateTime fromDate, DateTime toDate)
        {
            var dbo = comandDA.Repository;
            var commands = dbo.CustomerOrders
                 .Where(t => t.CustomerContractID.HasValue)
                 .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                          .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                 .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                 .Where(t => t.ServiceID == serviceId)
                 .SelectMany(t => t.CustomerContract.ServiceCommandContracts)
                 .Select(t => t.ServiceCommand)
                 .Where(t => t.IsAccept && t.IsApproval)
                 .Distinct()
                 .Select(item => new ServiceCommandItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     EndTime = item.EndTime,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Note = item.Note,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Filter(filter)
                 .ToList();
            return commands;
        }
        public List<ServiceCommandItem> GetTotalCommandNotApproval(ModelFilter filter, int serviceId, DateTime fromDate, DateTime toDate)
        {
            var dbo = comandDA.Repository;
            var commands = dbo.CustomerOrders
                 .Where(t => t.CustomerContractID.HasValue)
                 .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                 .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                  .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                 .Where(t => t.ServiceID == serviceId)
                 .SelectMany(t => t.CustomerContract.ServiceCommandContracts)
                 .Select(t => t.ServiceCommand)
                 .Where(t => !t.IsAccept && t.IsApproval)
                 .Distinct()
                 .Select(item => new ServiceCommandItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     EndTime = item.EndTime,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Note = item.Note,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Filter(filter)
                 .ToList();
            return commands;
        }
        public List<ServiceCommandItem> GetTotalCommandDoing(ModelFilter filter, int serviceId, DateTime fromDate, DateTime toDate)
        {
            var dbo = comandDA.Repository;
            var commands = dbo.CustomerOrders
                 .Where(t => t.CustomerContractID.HasValue)
                 .Where(t => !t.CustomerContract.IsFinish)
                 .Where(t => !t.CustomerContract.IsPause)
                 .Where(t => t.CustomerContract.IsStart)
                 .Where(t => !t.CustomerContract.IsCancel)
                 .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                 .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                 .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                 .Where(t => t.ServiceID == serviceId)
                 .SelectMany(t => t.CustomerContract.ServiceCommandContracts)
                 .Select(t => t.ServiceCommand)
                 .Where(t => t.IsAccept && t.IsApproval)
                 .Distinct()
                 .Select(item => new ServiceCommandItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     EndTime = item.EndTime,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Note = item.Note,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Filter(filter)
                 .ToList();
            return commands;
        }
        public List<ServiceCommandItem> GetTotalCommandPause(ModelFilter filter, int serviceId, DateTime fromDate, DateTime toDate)
        {
            var dbo = comandDA.Repository;
            var commands = dbo.CustomerOrders
                 .Where(t => t.CustomerContractID.HasValue)
                 .Where(t => !t.CustomerContract.IsFinish)
                 .Where(t => t.CustomerContract.IsPause && !t.CustomerContract.IsCancel)
                 .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                 .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                 .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                 .Where(t => t.ServiceID == serviceId)
                 .SelectMany(t => t.CustomerContract.ServiceCommandContracts)
                 .Select(t => t.ServiceCommand)
                 .Distinct()
                 .Select(item => new ServiceCommandItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     EndTime = item.EndTime,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Note = item.Note,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Filter(filter)
                 .ToList();
            return commands;
        }
        public List<ServiceCommandItem> GetTotalCommandCancel(ModelFilter filter, int serviceId, DateTime fromDate, DateTime toDate)
        {
            var dbo = comandDA.Repository;
            var commands = dbo.CustomerOrders
                 .Where(t => t.CustomerContractID.HasValue)
                 .Where(t => t.CustomerContract.IsCancel)
                 .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                 .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                 .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                 .Where(t => t.ServiceID == serviceId)
                 .SelectMany(t => t.CustomerContract.ServiceCommandContracts)
                 .Select(t => t.ServiceCommand)
                 .Distinct()
                 .Select(item => new ServiceCommandItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     EndTime = item.EndTime,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Note = item.Note,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Filter(filter)
                 .ToList();
            return commands;
        }
        public List<ServiceCommandItem> GetTotalCommandFinished(ModelFilter filter, int serviceId, DateTime fromDate, DateTime toDate)
        {
            var dbo = comandDA.Repository;
            var commands = dbo.CustomerOrders
                 .Where(t => t.CustomerContractID.HasValue)
                 .Where(t => t.CustomerContract.IsFinish)
                 .Where(t => !t.CustomerContract.IsCancel)
                 .Where(t => !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsDelete)
                 .Where(t => (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime <= toDate) || (t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.StartTime <= fromDate && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.EndTime >= toDate))
                 .Where(t => t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsApproval && t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsAccept && !t.CustomerContract.ServiceCommandContracts.FirstOrDefault().ServiceCommand.IsEdit)
                 .Where(t => t.ServiceID == serviceId)
                 .SelectMany(t => t.CustomerContract.ServiceCommandContracts)
                 .Select(t => t.ServiceCommand)
                 .Where(t => t.IsAccept && t.IsApproval && !t.IsEdit)
                 .Distinct()
                 .Select(item => new ServiceCommandItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     EndTime = item.EndTime,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Note = item.Note,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Filter(filter)
                 .ToList();
            return commands;
        }
    }
}
