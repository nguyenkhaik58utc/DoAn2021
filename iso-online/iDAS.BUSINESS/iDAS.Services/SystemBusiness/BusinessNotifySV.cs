using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
namespace iDAS.Services
{
    public class BusinessNotifySV
    {
        private BusinessNotifyDA notifyDA = new BusinessNotifyDA();
        public List<BusinessNotifyItem> GetByModule(ModelFilter filter, int employeeID, string module)
        {
            var dbo = notifyDA.Repository;
            var notifies = notifyDA.GetQuery()
                            .Where(i => i.HumanEmployeeID == employeeID && i.ModuleCode.Equals(module))
                            .OrderByDescending(i => i.CreateAt)
                            .Filter(filter)
                            .ToList();
            List<BusinessNotifyItem> lst = new List<BusinessNotifyItem>();
            if (notifies.Count > 0)
            {
                foreach (var item in notifies)
                {
                    var moduleItem = dbo.BusinessModules.FirstOrDefault(o => o.Code == item.ModuleCode);
                    var functionItem = dbo.BusinessFunctions.FirstOrDefault(i => item.FunctionName.Equals(i.Url));
                    lst.Add(
                         new BusinessNotifyItem()
                         {
                             ID = item.ID,
                             ModuleCode = item.ModuleCode,
                             ModuleName = moduleItem != null
                             ? moduleItem.Name
                                    + GetNotifyNewTotal(employeeID, item.ModuleCode) : string.Empty,
                             Content = item.Content,
                             FunctionName = item.FunctionName,
                             Param = item.Param,
                             Icon = functionItem.Icon,
                             FuctionCode = functionItem.Code,
                             FunctionTitle = functionItem.Name,
                             IsRead = item.IsRead,
                             CreateAt = item.CreateAt,
                             CreateBy = item.CreateBy,
                             DateSend = item.CreateAt.HasValue ? item.CreateAt.Value.ToString("dd/MM/yyyy") + " vào lúc "
                                        + item.CreateAt.Value.ToString("HH:mm tt") : string.Empty,
                             Title = item.Title
                         });
                }
            }
            return lst;
        }
        public BusinessNotifyItem GetByID(int id, string systemCode)
        {
            var dbo = notifyDA.Repository;
            var notifies = notifyDA.GetById(id);
            BusinessNotifyItem obj = new BusinessNotifyItem();
            obj.ID = notifies.ID;
            obj.ModuleCode = notifies.ModuleCode;
            obj.ModuleName = dbo.BusinessModules.FirstOrDefault(o => o.Code == notifies.ModuleCode) != null ?
                dbo.BusinessModules.FirstOrDefault(o => o.Code == notifies.ModuleCode).Name : string.Empty;
            obj.Content = notifies.Content;
            obj.IsRead = notifies.IsRead;
            obj.CreateAt = notifies.CreateAt;
            obj.CreateBy = notifies.CreateBy;
            obj.EmployeeSend = dbo.HumanUsers.Where(t => t.ID == notifies.CreateBy.Value).Select(i => i.HumanEmployee.Name).FirstOrDefault();
            obj.DateSend = notifies.CreateAt.HasValue ? notifies.CreateAt.Value.ToString("dd/MM/yyyy") + " vào lúc " + notifies.CreateAt.Value.ToString("HH:mm tt") : string.Empty;
            obj.Title = notifies.Title;
            return obj;
        }
        public List<BusinessNotifyItem> getNotifyUnRead(int employeeID)
        {
            var dbo = notifyDA.Repository;
            var notifies = notifyDA.GetQuery(i => !i.IsRead)
                            .OrderByDescending(i => i.CreateAt)
                            .Where(i => i.HumanEmployeeID == employeeID)
                            .ToList();
            List<BusinessNotifyItem> lst = new List<BusinessNotifyItem>();
            if (notifies.Count > 0)
            {
                foreach (var item in notifies)
                {
                    var moduleItem = dbo.BusinessModules.FirstOrDefault(o => o.Code == item.ModuleCode);
                    var functionItem = dbo.BusinessFunctions.FirstOrDefault(i => item.FunctionName.Equals(i.Url));
                    lst.Add(
                         new BusinessNotifyItem()
                         {
                             ID = item.ID,
                             ModuleCode = item.ModuleCode,
                             ModuleName = moduleItem != null
                             ? moduleItem.Name : string.Empty,
                             Content = item.Content,
                             FunctionName = item.FunctionName,
                             Param = item.Param,
                             Icon = functionItem != null ? functionItem.Icon : string.Empty,
                             FuctionCode = functionItem != null ? functionItem.Code : string.Empty,
                             FunctionTitle = functionItem != null ? functionItem.Name : string.Empty,
                             IsRead = item.IsRead,
                             CreateAt = item.CreateAt,
                             CreateBy = item.CreateBy,
                             DateSend = item.CreateAt.HasValue ? item.CreateAt.Value.ToString("dd/MM/yyyy") : string.Empty,
                             TimeSend = item.CreateAt.HasValue ? item.CreateAt.Value.ToString("HH:mm tt") : string.Empty,
                             Title = item.Title + "<br/><p style = 'margin-top: 3pt; margin-bottom: 3pt; font-size: 0.8em; color: #a7a7a7; clear:left'>Gửi ngày "
                                                 + item.CreateAt.Value.ToString("dd/MM/yyyy") + " vào lúc "
                                                 + item.CreateAt.Value.ToString("HH:mm tt")
                                                 + "</p><p style = 'margin-top: 0pt; margin-bottom: 0pt;  clear:left font-size: 0.8em; color: #a7a7a7;'>"
                                                 + item.Content + "</p>",
                         });
                }
            }
            return lst;
        }
        public List<BusinessNotifyItem> GetNotify(ModelFilter filter, int employeeID, bool isAll = false)
        {
            var dbo = notifyDA.Repository;
            IQueryable<Notify> query = dbo.Notifies;
            if (!isAll)
            {
                query = query.Where(i => !i.IsRead);
            }
            var notifies = query
                            .OrderByDescending(i => i.CreateAt)
                            .Where(i => i.HumanEmployeeID == employeeID)
                            .Filter(filter)
                            .ToList();
            List<BusinessNotifyItem> lst = new List<BusinessNotifyItem>();
            if (notifies.Count > 0)
            {
                foreach (var item in notifies)
                {
                    var moduleItem = dbo.BusinessModules.FirstOrDefault(o => o.Code == item.ModuleCode);
                    var functionItem = dbo.BusinessFunctions.FirstOrDefault(i => item.FunctionName.Equals(i.Url));
                    lst.Add(
                         new BusinessNotifyItem()
                         {
                             ID = item.ID,
                             ModuleCode = item.ModuleCode,
                             ModuleName = moduleItem != null
                             ? moduleItem.Name
                                    + GetNotifyNewTotal(employeeID, item.ModuleCode) : string.Empty,
                             Content = item.Content,
                             FunctionName = item.FunctionName,
                             Param = item.Param,
                             Icon = functionItem != null ? functionItem.Icon : string.Empty,
                             FuctionCode = functionItem != null ? functionItem.Code : string.Empty,
                             FunctionTitle = functionItem != null ? functionItem.Name : string.Empty,
                             IsRead = item.IsRead,
                             CreateAt = item.CreateAt,
                             CreateBy = item.CreateBy,
                             DateSend = item.CreateAt.HasValue ? item.CreateAt.Value.ToString("dd/MM/yyyy") : string.Empty,
                             TimeSend = item.CreateAt.HasValue ? item.CreateAt.Value.ToString("HH:mm tt") : string.Empty,
                             Title = item.Title + "<br/><p style = 'margin-top: 3pt; margin-bottom: 3pt; font-size: 0.8em; color: #a7a7a7; clear:left'>Gửi ngày "
                                                 + item.CreateAt.Value.ToString("dd/MM/yyyy") + " vào lúc "
                                                 + item.CreateAt.Value.ToString("HH:mm tt")
                                                 + "</p><p style = 'margin-top: 0pt; margin-bottom: 0pt;  clear:left font-size: 0.8em; color: #a7a7a7;'>"
                                                 + item.Content + "</p>",
                         });
                }
            }
            return lst;
        }
        public string GetNotifyNewTotal(int employeeID, string moduleCode)
        {
            var total = notifyDA.GetQuery(t => t.ModuleCode == moduleCode && !t.IsRead && t.HumanEmployeeID == employeeID).Count();
            var data = total > 0 ? " (" + total.ToString() + " thông báo chưa đọc)" : string.Empty;
            return data;
        }
        public string GetNotifyNewTotal(int employeeID)
        {
            var total = notifyDA.GetQuery(i => !i.IsRead && i.HumanEmployeeID == employeeID).Count();
            var data = total > 0 ? " (" + total.ToString() + ")" : string.Empty;
            return data;
        }
        public void Insert(BusinessNotifyItem notify)
        {
            var notifies = new List<Notify>();
            foreach (var employeeID in notify.EmployeeIDs)
            {
                var item = new Notify()
                {
                    HumanEmployeeID = employeeID,
                    ModuleCode = notify.ModuleCode,
                    Title = notify.Title,
                    Content = notify.Content,
                    IsRead = notify.IsRead,
                    FunctionName = notify.FunctionName,
                    Param = notify.Param,
                    ReadTime = notify.ReadTime,
                    CreateAt = notify.CreateAt,
                    CreateBy = notify.CreateBy,
                    UpdateAt = notify.UpdateAt,
                    UpdateBy = notify.UpdateBy,
                };
                notifies.Add(item);
            }
            notifyDA.InsertRange(notifies);
            notifyDA.Save();
        }
        public void UpdateRead(int id, int userId)
        {
            var obj = notifyDA.GetById(id);
            obj.IsRead = true;
            obj.ReadTime = DateTime.Now;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            notifyDA.Update(obj);
            notifyDA.Save();
        }
        public void UpdateAllRead(string module, int userId)
        {
            var dbo = notifyDA.Repository;
            var data = notifyDA.GetQuery(t => t.ModuleCode.Equals(module) && !t.IsRead).ToList();
            foreach (var item in data)
            {
                item.IsRead = true;
                item.ReadTime = DateTime.Now;
                item.UpdateAt = DateTime.Now;
                item.UpdateBy = userId;
                notifyDA.Update(item);
            }
            notifyDA.Save();
        }
        public void UnRead(string stringId, int userID)
        {
            List<int> ids = stringId.Split(',').Select(n => int.Parse(n)).ToList();
            var notifies = notifyDA.GetQuery(i => ids.Contains(i.ID)).ToList();
            foreach (var notify in notifies)
            {
                notify.IsRead = false;
                notify.UpdateAt = DateTime.Now;
                notify.UpdateBy = userID;
                notifyDA.Update(notify);
            }
            notifyDA.Save();
        }
        public void ReadMany(string stringId, int userID)
        {
            List<int> ids = stringId.Split(',').Select(n => int.Parse(n)).ToList();
            var notifies = notifyDA.GetQuery(i => ids.Contains(i.ID)).ToList();
            foreach (var notify in notifies)
            {
                notify.IsRead = true;
                notify.UpdateAt = DateTime.Now;
                notify.UpdateBy = userID;
                notifyDA.Update(notify);
            }
            notifyDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            notifyDA.DeleteRange(ids);
            notifyDA.Save();
        }
    }
}
