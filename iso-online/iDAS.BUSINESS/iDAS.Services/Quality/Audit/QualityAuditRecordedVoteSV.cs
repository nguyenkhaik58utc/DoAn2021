using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
   public class QualityAuditRecordedVoteSV
    {
       private QualityAuditRecordedVoteDA QualityAuditRecordedVoteDA = new QualityAuditRecordedVoteDA();
       public void Insert(QualityAuditRecordedVoteItem objIn, int userId)
       {
           var obj = new QualityAuditRecordedVote();
           obj.HumanDepartmentID = objIn.HumanDepartmentID;
           obj.QualityAuditProgramID = objIn.QualityAuditProgramID;
           obj.AuditBy= objIn.Auditer.ID;
           obj.AuditAt = objIn.AuditAt;
           obj.Contents = objIn.Contents;
           obj.CreateAt = DateTime.Now;
           obj.CreateBy= userId;
           QualityAuditRecordedVoteDA.Insert(obj);
           QualityAuditRecordedVoteDA.Save();
       }
       public void Update(QualityAuditRecordedVoteItem objEdit, int userId)
       {
           var obj = QualityAuditRecordedVoteDA.GetById(objEdit.ID);
           obj.HumanDepartmentID = objEdit.HumanDepartmentID;
           obj.QualityAuditProgramID = objEdit.QualityAuditProgramID;
           obj.AuditBy = objEdit.Auditer.ID;
           obj.AuditAt = objEdit.AuditAt;
           obj.Contents = objEdit.Contents;
           obj.UpdateAt = DateTime.Now;
           obj.UpdateBy = userId;
           QualityAuditRecordedVoteDA.Update(obj);
           QualityAuditRecordedVoteDA.Save();
       }
       public QualityAuditRecordedVoteItem GetByID(int id)
       {
           var employeeSV = new HumanEmployeeSV();
           QualityAuditRecordedVoteItem data = new QualityAuditRecordedVoteItem();
           var obj = QualityAuditRecordedVoteDA.GetById(id);
           if(obj!=null)
           {
               data.HumanDepartmentID = obj.HumanDepartmentID;
               data.QualityAuditProgramID = obj.QualityAuditProgramID;
               data.Contents = obj.Contents;
               data.AuditAt = obj.AuditAt;
               data.ID = obj.ID;
               data.Auditer = employeeSV.GetEmployeeView(obj.AuditBy);
           }
           return data;
       }
       public void Delete(string stringId)
       {
           var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
           QualityAuditRecordedVoteDA.DeleteRange(ids);
           QualityAuditRecordedVoteDA.Save();
       }
    }
}
