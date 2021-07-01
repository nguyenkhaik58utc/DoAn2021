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
    public class ServiceStageSV
    {
        private ServiceStageDA phaseDA = new ServiceStageDA();
        public List<ServiceStageItem> GetAllByServiceID(int page, int pageSize, out int total, int serviceID)
        {
            try
            {
                List<ServiceStageItem> lst = new List<ServiceStageItem>();
                var lstPhase = phaseDA.GetQuery(t => t.ServiceID == serviceID).OrderBy(t => t.Order).Page(page, pageSize, out total).ToList();
                if (lstPhase != null && lstPhase.Count > 0)
                {
                    foreach (var item in lstPhase)
                    {
                        lst.Add(new ServiceStageItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            CreateAt = item.CreateAt,                         
                            Order = (int)item.Order,
                            IsUse = item.IsUse,
                            CreateBy = item.CreateBy,
                            ServiceID = item.ServiceID,
                            Time = item.Time,
                            TimeUnitID = item.TimeUnit,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ServiceStageItem GetByID(int id)
        {
            var phase = phaseDA.GetById(id);
            ServiceStageItem obj = new ServiceStageItem();
            obj.ID = phase.ID;
            obj.Name = phase.Name;
            obj.Note = phase.Note;          
            obj.ServiceID = phase.ServiceID;
            obj.Time = phase.Time;
            obj.TimeUnitID = phase.TimeUnit;
            obj.IsUse = phase.IsUse;
            obj.Order = (int)phase.Order;
            obj.CreateBy = phase.CreateBy;
            obj.UpdateAt = phase.UpdateAt;
            obj.UpdateBy = phase.UpdateBy;
            obj.CreateAt = phase.CreateAt;
            return obj;
        }
        public void Insert(ServiceStageItem objNew, int userId)
        {
            ServiceStage obj = new ServiceStage();
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.IsUse = objNew.IsUse;
            obj.Name = objNew.Name;            
            obj.Note = objNew.Note;
            obj.Order = objNew.Order;
            obj.ServiceID = objNew.ServiceID;
            obj.Time = objNew.Time;
            obj.TimeUnit = objNew.TimeUnitID;
            phaseDA.Insert(obj);
            phaseDA.Save();
        }
        public void Update(ServiceStageItem objEdit, int userId)
        {
            ServiceStage obj = phaseDA.GetById(objEdit.ID);
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.IsUse = objEdit.IsUse;
            obj.Name = objEdit.Name;         
            obj.Note = objEdit.Note;
            obj.Order = objEdit.Order;
            obj.ServiceID = objEdit.ServiceID;
            obj.Time = objEdit.Time;
            obj.TimeUnit = objEdit.TimeUnitID;
            phaseDA.Update(obj);
            phaseDA.Save();
        }
        public void Delete(int id)
        {
            var obj = phaseDA.GetById(id);
            phaseDA.Delete(obj);
            phaseDA.Save();
        }
    }
}
