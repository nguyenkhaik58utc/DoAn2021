using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class HumanResultQuestionSV
    {
        private HumanResultQuestionDA humanResultQuestionDA = new HumanResultQuestionDA();
        private HumanResultAnswerDA humanResultAnswerDA = new HumanResultAnswerDA();
        private HumanEmployeeAuditSV humanEmployeeAuditSV = new HumanEmployeeAuditSV();
        private bool GetResult(int questionId, int[] strAnswerID)
        {
            var dbo = humanResultQuestionDA.Repository;
            var contractIDs = strAnswerID;
            var s = dbo.HumanAnswers.Where(t => t.IsTrue && t.HumanQuestionID == questionId).Select(t => t.ID).ToArray();
            bool result = Enumerable.SequenceEqual(contractIDs, s);
            return result;
        }
        public List<HumanResultQuestionItem> GetResultQuestion(ModelFilter filter, int cateId, int employeeId, int humanEmployeeAuditID)
        {
            var dbo = humanResultQuestionDA.Repository;
            List<HumanResultQuestionItem> lst = new List<HumanResultQuestionItem>();
            var data = dbo.HumanQuestions.Where(t => t.HumanCategoryQuestionID == cateId && t.IsUse && !t.IsDelete).Filter(filter).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new HumanResultQuestionItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ResultName = dbo.HumanResultQuestions.Where(t => t.HumanQuestionID == item.ID && t.HumanEmployeeAuditID == humanEmployeeAuditID).FirstOrDefault() != null ? dbo.HumanResultQuestions.Where(t => t.HumanQuestionID == item.ID && t.HumanEmployeeAuditID == humanEmployeeAuditID).FirstOrDefault().IsResult ? "Đúng" : "Sai" : "Chưa trả lời"
                    });
                }
            }
            return lst;
        }
        public int GetIDByHumanEmployeeAuditIDandHumanQuestionID(int humanEmployeeAuditId, int humanQuestionId)
        {
            var dbo = humanResultQuestionDA.Repository;
            var s = dbo.HumanResultQuestions.Where(t => t.HumanEmployeeAuditID == humanEmployeeAuditId && t.HumanQuestionID == humanQuestionId).FirstOrDefault();
            if (s != null)
                return s.ID;
            return 0;
        }
        private int GetHumanResultQuestionID(int humanEmployeeAuditId, int humanQuestionId)
        {
            var dbo = humanResultQuestionDA.Repository;
            var s = dbo.HumanResultQuestions.Where(t => t.HumanEmployeeAuditID == humanEmployeeAuditId && t.HumanQuestionID == humanQuestionId).FirstOrDefault();
            if (s != null)
                return s.ID;
            return 0;
        }
        public void InsertResultQuestion(HumanResultQuestionItem objNew, int[] strAnswerID, int phaseAuditID, int userId, int employeeId)
        {
            var dbo = humanResultQuestionDA.Repository;
            var data = dbo.HumanQuestions.Where(t => t.HumanAnswers.Where(x => strAnswerID.Contains(x.ID)).Count() > 0).Select(t => t.ID).ToArray();
            if (data.Count() > 0)
            {
                foreach (var x in data)
                {
                    var humanEmployeeAuditId = humanEmployeeAuditSV.GetByPhaseAuditIDAndEmployeeID(phaseAuditID, employeeId);
                    int humanResultQuestionID = GetHumanResultQuestionID(humanEmployeeAuditId, x);
                    if (humanResultQuestionID == 0)
                    {
                        var obj = new HumanResultQuestion();
                        obj.HumanEmployeeAuditID = humanEmployeeAuditId;
                        obj.HumanQuestionID = x;
                        obj.IsResult = GetResult(x, strAnswerID);
                        obj.CreateAt = DateTime.Now;
                        obj.CreateBy = userId;
                        humanResultQuestionDA.Insert(obj);
                        humanResultQuestionDA.Save();
                        if (strAnswerID != null && strAnswerID.Count() > 0)
                        {
                            foreach (var item in strAnswerID)
                            {


                                HumanResultAnswer contract = new HumanResultAnswer();
                                contract.HumanResultQuestionID = obj.ID;
                                contract.HumanAnswerID = (int)item;
                                contract.CreateAt = DateTime.Now;
                                contract.CreateBy = userId;
                                humanResultAnswerDA.Insert(contract);
                            }
                            humanResultAnswerDA.Save();
                        }
                       // return obj.ID;
                    }
                    else
                    {
                        var obj = humanResultQuestionDA.GetById(humanResultQuestionID);
                        obj.HumanEmployeeAuditID = humanEmployeeAuditId;
                        obj.HumanQuestionID = x;
                        obj.IsResult = GetResult(x, strAnswerID);
                        obj.UpdateAt = DateTime.Now;
                        obj.UpdateBy = userId;
                        humanResultQuestionDA.Update(obj);
                        humanResultQuestionDA.Save();
                        dbo.HumanResultAnswers.RemoveRange(dbo.HumanResultAnswers.Where(f => f.HumanResultQuestionID == humanResultQuestionID));
                        dbo.SaveChanges();
                        if (strAnswerID != null && strAnswerID.Count() > 0)
                        {
                            foreach (var item in strAnswerID)
                            {
                                HumanResultAnswer contract = new HumanResultAnswer();
                                contract.HumanResultQuestionID = obj.ID;
                                contract.HumanAnswerID = (int)item;
                                contract.CreateAt = DateTime.Now;
                                contract.CreateBy = userId;
                                humanResultAnswerDA.Insert(contract);
                            }
                            humanResultAnswerDA.Save();
                        }
                      //  return obj.ID;
                    }
                }
            }
        }
    }
}
