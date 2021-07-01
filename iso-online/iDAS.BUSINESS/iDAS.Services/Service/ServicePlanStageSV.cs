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
   public class ServicePlanStageSV
    {
       private ServicePlanStageDA planPhaseDA = new ServicePlanStageDA();
       private ServiceStageDA phaseDA = new ServiceStageDA();
       private ServicePlanSV planService = new ServicePlanSV();
       private TaskSV taskService = new TaskSV();
       public int CheckExits(int servicePlanId, int customerId, int stageId)
       {
           var rs = planPhaseDA.GetQuery(t => t.ServicePlanID == servicePlanId && t.ServiceStageID == stageId && t.CustomerID==customerId).FirstOrDefault();
           if (rs != null)
               return rs.ID;
           return 0;

       }
       public List<ServicePlanItem> GetForAccounting(int page, int pageSize, out int totalCount, int contractId)
       {
           return planService.GetForAccounting(page, pageSize, out totalCount, contractId);
       }
       public List<ServicePlanItem> GetAllByPlanParentIDIsChoice(int page, int pageSize, out int totalCount, int parentId)
       {
           return planService.GetAllByPlanParentIDIsChoice(page, pageSize, out totalCount, parentId);
       }
       public void Delete(int id)
       {
           var rs = planPhaseDA.GetById(id);
           planPhaseDA.Delete(rs);
           planPhaseDA.Save();

       }

       public void Insert(int servicePlanId, int criteriaId, int customerId, int userId)
       {
           var obj = new ServicePlanStage();
           obj.ServiceStageID = criteriaId;
           obj.ServicePlanID = servicePlanId;
           obj.CustomerID = customerId;
           obj.CreateAt = DateTime.Now;
           obj.CreateBy = userId;
           planPhaseDA.Insert(obj);
           planPhaseDA.Save();
       }
       public List<ServicePlanStageItem> GetAllByServiceID(int page, int pageSize, out int totalCount, int serviceId, int planId)
        {
            var dbo = planPhaseDA.Repository;
            List<ServicePlanStageItem> lst = new List<ServicePlanStageItem>();
            var phases = phaseDA.GetQuery(t => t.ServiceID == serviceId && t.IsUse)
                          .OrderBy(t => t.Order)
                          .Page(page, pageSize, out totalCount)
                          .ToList();
            foreach (var item in phases)
            {
                lst.Add(new ServicePlanStageItem
                {
                    ID = item.ID,
                    Order =(int)dbo.ServiceStages.Where(t=>t.ID==item.ID).FirstOrDefault().Order,
                    StageName = item.Name, 
                    ServiceName = item.Service.Name,
                    //Task = taskService.GetByIDForPlan(GetTaskID(planId, item.ID)),
                    //IsSelected = taskService.GetByIDForPlan(GetTaskID(planId, item.ID)).ID!=0 ?true :false

                });

            }
            return lst;
        }
       
    }
}
