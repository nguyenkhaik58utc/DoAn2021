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
    public class HumanCategoryQuestionSV
    {
        private HumanCategoryQuestionDA criteriaCategoryDA = new HumanCategoryQuestionDA();
        public List<HumanCategoryQuestionItem> GetCombobox()
        {
            var dbo = criteriaCategoryDA.Repository;
            var checks = criteriaCategoryDA.GetQuery(t => !t.IsDelete && t.IsUse)
                          .Select(item => new HumanCategoryQuestionItem()
                          {
                              ID = item.ID,
                              IsDelete = item.IsDelete,
                              Name = item.Name,
                              ParentID = item.ParentID,
                              CreateAt = item.CreateAt,
                              NumberQuestion = dbo.HumanQuestions.Where(t => t.HumanCategoryQuestionID == item.ID && !t.IsDelete && t.IsUse).Count()

                          })

                          .OrderByDescending(item => item.CreateAt)
                          .ToList();
            return checks;
        }
        public List<HumanCategoryQuestionItem> GetData(int employeeId, bool isAdminstrator = false)
        {

            var checks = criteriaCategoryDA.GetQuery(t => !t.IsDelete)
                          .Select(item => new HumanCategoryQuestionItem()
                          {
                              ID = item.ID,
                              IsDelete = item.IsDelete,
                              Name = item.Name,
                              ParentID = item.ParentID,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .ToList();
            return checks;
        }
        public List<TreeCriteriaCategoryItem> GetTreeData(string nodeId)
        {
            int parentId = nodeId == "root" ? 0 : Convert.ToInt32(nodeId);
            var dbo = criteriaCategoryDA.Repository;
            var lsDataNode = dbo.HumanCategoryQuestions
                                .Where(i => i.ParentID == parentId && i.IsDelete == false)
                                .Select(item => new TreeCriteriaCategoryItem()
                                {
                                    ID = item.ID,
                                    IsDelete = item.IsDelete,
                                    Name = item.Name,
                                    ParentID = item.ParentID,
                                    IsUse = item.IsUse,
                                    Leaf = dbo.HumanCategoryQuestions.FirstOrDefault(i => i.ParentID == item.ID) == null
                                }
                                ).ToList();
            return lsDataNode;
        }
        public List<TreeCriteriaCategoryItem> GetProductionCategory(int parentId)
        {
            var dbo = criteriaCategoryDA.Repository;
            var lsDataNode = dbo.QualityCriteriaCategories
                                .Where(i => i.ParentID == parentId && i.IsProduction && !i.IsDelete)
                                .Select(item => new TreeCriteriaCategoryItem()
                                {
                                    ID = item.ID,
                                    IsDelete = item.IsDelete,
                                    Name = item.Name,
                                    ParentID = item.ParentID,
                                    IsUse = item.IsActive,
                                    Leaf = dbo.QualityCriteriaCategories.Where(i => i.IsProduction && i.ParentID == item.ID).FirstOrDefault() == null
                                }
                                ).ToList();
            return lsDataNode;
        }
        public HumanCategoryQuestionItem GetById(int id)
        {
            var dbo = criteriaCategoryDA.Repository;
            var employeeSV = new HumanEmployeeSV();
            var assessCategories = criteriaCategoryDA.GetById(id);
            var obj = new HumanCategoryQuestionItem();
            if (assessCategories != null)
            {
                obj.ID = assessCategories.ID;
                obj.Name = assessCategories.Name;
                obj.ParentID = assessCategories.ParentID;
                obj.IsUse = assessCategories.IsUse;
                obj.IsDelete = assessCategories.IsDelete;
                obj.CreateAt = assessCategories.CreateAt;
                obj.CreateBy = assessCategories.CreateBy;
                obj.UpdateBy = assessCategories.UpdateBy;
                obj.UpdateAt = assessCategories.UpdateAt;
            }
            return obj;
        }
        public bool Insert(HumanCategoryQuestionItem objNew)
        {
            var dbo = criteriaCategoryDA.Repository;
            HumanCategoryQuestion obj = new HumanCategoryQuestion();
            if (dbo.QualityCriteriaCategories.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper()) != null)
            {
                return false;
            }
            obj.Name = objNew.Name;
            obj.ParentID = objNew.ParentID;
            obj.IsUse = objNew.IsUse;
            obj.IsDelete = objNew.IsDelete;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = objNew.CreateBy;
            dbo.HumanCategoryQuestions.Add(obj);
            dbo.SaveChanges();
            return true;
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            criteriaCategoryDA.DeleteRange(ids);
            criteriaCategoryDA.Save();
        }
        public bool Delete(int id)
        {
            var dbo = criteriaCategoryDA.Repository;
            if (dbo.QualityCriteriaCategories.FirstOrDefault(i => i.ParentID == id) != null)
            {
                dbo.QualityCriteriaCategories.RemoveRange(dbo.QualityCriteriaCategories.Where(i => i.ID == id));
                dbo.SaveChanges();
                return true;
            }
            else
            {
                var obj = criteriaCategoryDA.GetById(id);
                criteriaCategoryDA.Delete(obj);
                criteriaCategoryDA.Save();
                return true;
            }
            return false;
        }
        public bool Update(HumanCategoryQuestionItem objEdit, int userId)
        {
            try
            {
                var dbo = criteriaCategoryDA.Repository;
                HumanCategoryQuestion obj = dbo.HumanCategoryQuestions.FirstOrDefault(i => i.ID == objEdit.ID);
                if (dbo.QualityCriteriaCategories.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
                     && (obj.Name.ToUpper() != objEdit.Name.ToUpper()))
                {
                    return false;
                }
                obj.Name = objEdit.Name;
                obj.ParentID = objEdit.ParentID;
                obj.IsUse = objEdit.IsUse;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = userId;
                dbo.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CheckNameExits(string name)
        {
            try
            {
                var rs = criteriaCategoryDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).ToList();
                if (rs.Count > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<HumanCategoryQuestionItem> GetGroupsByParentID(int parentId)
        {
            var groups = criteriaCategoryDA.GetQuery(t => t.ParentID == parentId)
                           .Select(item => new HumanCategoryQuestionItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               Name = item.Name
                           }).ToList();

            return groups;
        }
        public void GetGroupsByParentID(int id, ref List<int> groupIds)
        {
            var groups = GetGroupsByParentID(id);
            if (groups.Count <= 0) return;
            foreach (var group in groups)
            {
                groupIds.Add(group.ID);
                GetGroupsByParentID(group.ID, ref groupIds);
            }
        }
        public void UseIDASData(string strQuestionCateIds, int parentId)
        {
            var questionCateIds = strQuestionCateIds.Split(',').Select(i => int.Parse(i)).ToList();
            CenterAuditCategoryQuestionDA centerCategoryQuestionDA = new CenterAuditCategoryQuestionDA();
            var questionCates = centerCategoryQuestionDA.GetQuery(i => questionCateIds.Contains(i.ID)).ToList();
            //var questions = questionCates.SelectMany(i => i.CenterAuditQuestions).ToList();
            //var answers = questions.Select(i => i.CenterAuditAnswers).ToList();
            criteriaCategoryDA.InsertRange(
                   questionCates
                        .Select(item => new HumanCategoryQuestion()
                                         {
                                             ParentID = parentId,
                                             Name = item.Name,
                                             Note = item.Note,
                                             IsUse = item.IsUse,
                                             HumanQuestions = item.CenterAuditQuestions.Where(q => q.IsUse)
                                                                     .Select(
                                                                                q => new HumanQuestion()
                                                                                            {
                                                                                                Name = q.Name,
                                                                                                Note = q.Note,
                                                                                                HumanAnswers = q.CenterAuditAnswers
                                                                                                                .Select(
                                                                                                                    a => new HumanAnswer()
                                                                                                                    {
                                                                                                                        Answer = a.Answer,
                                                                                                                        IsTrue = a.IsTrue
                                                                                                                    }
                                                                                                                ).ToList()
                                                                                            }
                                                                     ).ToList()
                                         }
                                ).ToList()
                );
            criteriaCategoryDA.Save();
        }
    }
}
