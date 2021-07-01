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
    public class AuditSV
    {
        private AuditDA auditDA = new AuditDA();
        private CenterISOStandardDA iSOStandardDA = new CenterISOStandardDA();
        private ISOStandardSV iSOStandardService = new ISOStandardSV();
        //private AuditDetailService auditDetailService = new AuditDetailService();
        private AuditResultNCSV auditResultIncorrectService = new AuditResultNCSV();
        private TaskSV taskService = new TaskSV();
        #region-- Code này nên vứt vào Target
        public int InsertForTarget(AuditItem objNew, int userId, int employeeId)
        {
            Audit obj = new Audit();
            obj.IsPass = objNew.IsPass;
            //obj.CriteriCategoryID = (int)objNew.CriteriaCategoryID;
            //obj.AuditAt = objNew.AuditAt;
            //obj.AuditBy = employeeId;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            auditDA.Insert(obj);
            auditDA.Save();
            return obj.ID;
        }
        
        //public List<AuditItem> GetAllForPlanPhase(int page, int pageSize, out int total, int paramRaw)
        //{
        //    int[] arrayAuditId = planPhaseAuditService.GetAuditIDByPlanPhaseID(paramRaw);
        //    var audits = auditDA.GetQuery(t => arrayAuditId.Contains(t.ID))
        //                    .OrderByDescending(item => item.CreateAt)
        //                    .Page(page, pageSize, out total)
        //                    .ToList();
        //    List<AuditItem> lst = new List<AuditItem>();
        //    if (audits.Count > 0)
        //    {
        //        foreach (var item in audits)
        //        {
        //            lst.Add(new AuditItem
        //            {
        //                ID = item.ID,
        //                AuditAt = item.AuditAt,
        //                //   Content = item.Content,
        //                CriteriaCategoryName = item.MnCriteriaCategory.Name,
        //                IsPass = item.IsPass,
        //                //  Note = item.Note,
        //                CreateAt = item.CreateAt,
        //                //Obs = auditIncorrectService.GetObsNumber(item.ID),
        //                //Maximum = auditIncorrectService.GetMaximumNumber(item.ID),
        //                //Medium = auditIncorrectService.GetMediumNumber(item.ID),

        //            });
        //        }
        //    }


        //    return lst;
        //}
        #endregion
        public AuditItem GetByID(int id)
        {
            var employeeSV = new HumanEmployeeSV();
            AuditItem obj = new AuditItem();
            var audit = auditDA.GetById(id);
            if (audit != null)
            {
                obj.ID = audit.ID;
                obj.Note = audit.Note;
                obj.QualityCriteriaCategoryID = audit.QualityCriteriaCategoryID;
                obj.Audit = employeeSV.GetEmployeeView(audit.QualityCriteriaCategory.EmployeeID);
                obj.Time = audit.Time;
                obj.IsPass = audit.IsPass;
            }
            return obj;
        }
        public AuditItem GetByTaskID(int taskId, int employeeId)
        {
            var employeeSV = new HumanEmployeeSV();
            var dbo = auditDA.Repository;
            AuditItem obj = new AuditItem();
            var audit = auditDA.GetQuery(t =>t.ID == t.Tasks.FirstOrDefault(x=>x.ID== taskId).AuditID.Value
                ).FirstOrDefault();
            if(audit==null)
            {
                obj.Audit = employeeSV.GetEmployeeView(employeeId);
                obj.TaskID = taskId;
                obj.DepartmentID = dbo.Tasks.FirstOrDefault(m => m.ID == taskId).HumanDepartmentID;
            }
            if (audit != null)
            {
                obj.ID = audit.ID;
                obj.TaskID = (int)audit.Tasks.FirstOrDefault().ID;
                obj.DepartmentID = audit.Tasks.FirstOrDefault(m => m.ID == taskId).HumanDepartmentID;
                obj.Note = audit.Note;
                obj.QualityCriteriaCategoryID = audit.QualityCriteriaCategoryID;
                obj.Audit = employeeSV.GetEmployeeView(audit.EmployeeID);
                obj.Time = audit.Time;
                obj.IsPass = audit.IsPass;
            }
            return obj;
        }
        public AuditItem GetByTaskCode(string taskCode)
        {
            var rs = auditDA.GetQuery().FirstOrDefault();
            if (rs == null)
                return null;
            else
            {
                AuditItem obj = new AuditItem();
                return obj;
            }
        }

        public int Insert(AuditItem objNew)
        {
            var dbo = auditDA.Repository;
            iDAS.Base.Audit obj = new iDAS.Base.Audit();
            obj.IsPass = objNew.IsPass;
            obj.Time = objNew.Time;
            obj.EmployeeID = objNew.EmployeeID;
            obj.IsPass = objNew.IsPass;
            obj.Note = objNew.Note;
            obj.QualityCriteriaCategoryID = objNew.QualityCriteriaCategoryID;
            obj.CreateAt = DateTime.Now;       
            auditDA.Insert(obj);    
           var task = dbo.Tasks.FirstOrDefault(t=>t.ID==objNew.TaskID);
           task.AuditID = obj.ID;
           auditDA.Save();
            return obj.ID;
        }
      

        public void Update(AuditItem objNew, int userId, int employeeId)
        {
            iDAS.Base.Audit obj = auditDA.GetById(objNew.ID);
            obj.IsPass = objNew.IsPass;
            //obj.TaskID = objNew.TaskID;
            obj.Note = objNew.Note;
            obj.QualityCriteriaCategoryID = objNew.QualityCriteriaCategoryID;
            obj.Time = objNew.Time;
           // obj.au = employeeId;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            auditDA.Update(obj);
            auditDA.Save();
        }

        public void Delete(int id)
        {
            var audits = auditDA.GetById(id);
            auditDA.Delete(audits);
            auditDA.Save();
        }
    }
}
