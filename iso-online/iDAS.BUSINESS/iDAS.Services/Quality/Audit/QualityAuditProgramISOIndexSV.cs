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
    public class QualityAuditProgramISOIndexSV
    {
        private QualityAuditProgramISOIndexDA qualityAuditProgramISODA = new QualityAuditProgramISOIndexDA();
        private CenterISOIndexDA iSOClauseDA = new CenterISOIndexDA();
        private string SystemCode = "BUSINESS";

        #region Code cũ
        public void InsertRange(List<QualityAuditProgramISOIndexItem> items, int userID)
        {
            var rprs = new List<QualityAuditProgramISO>();
            rprs = items.Select(item => new QualityAuditProgramISO()
            {

                QualityAuditProgramID = item.QualityAuditProgramID,
                ISOIndexID = item.ISOIndexID,
                CreateAt = DateTime.Now,
                CreateBy = userID

            }).ToList();
            qualityAuditProgramISODA.InsertRange(rprs);
            qualityAuditProgramISODA.Save();
        }
        public void UpdateRange(int[] arrIds, int programId, int userID)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            dbo.QualityAuditProgramISOes.RemoveRange(dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgramID == programId && !arrIds.Contains(t.ISOIndexID)).ToList());
            dbo.SaveChanges();
            var rprs = new List<QualityAuditProgramISO>();
            foreach (var arr in arrIds)
            {
                if (!dbo.QualityAuditProgramISOes.Any(t => t.QualityAuditProgramID == programId && t.ISOIndexID == arr))
                {
                    var rpr = new QualityAuditProgramISO();
                    rpr.ISOIndexID = arr;
                    rpr.QualityAuditProgramID = programId;
                    rprs.Add(rpr);
                }
            }
            qualityAuditProgramISODA.InsertRange(rprs);
            qualityAuditProgramISODA.Save();
        }
        public void RemoveAllClause(int programId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            dbo.QualityAuditProgramISOes.RemoveRange(dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgramID == programId).ToList());
            dbo.SaveChanges();
        }
        private int[] GetListNCIDByProgramISOID(int qualityAuditProgramIsoID)
        {
            try
            {
                var dbo = qualityAuditProgramISODA.Repository;
                var module = dbo.QualityAuditResults.Where(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoID).Join(dbo.QualityNCs, ac => ac.QualityNCID, a => a.ID, (ac, c) => c.ID).ToArray();
                return module;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetObsNumber(int qualityAuditProgramIsoID)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            int[] arrayID = GetListNCIDByProgramISOID(qualityAuditProgramIsoID);
            var data = dbo.QualityNCs.Where(t => arrayID.Contains(t.ID) && t.IsObs == true).ToList();
            return data.Count;

        }
        public int GetMaximumNumber(int qualityAuditProgramIsoID)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            int[] arrayID = GetListNCIDByProgramISOID(qualityAuditProgramIsoID);
            var data = dbo.QualityNCs.Where(t => arrayID.Contains(t.ID) && t.IsMaximum == true).ToList();
            return data.Count;
        }
        public int GetMediumNumber(int qualityAuditProgramIsoID)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            int[] arrayID = GetListNCIDByProgramISOID(qualityAuditProgramIsoID);
            var data = dbo.QualityNCs.Where(t => arrayID.Contains(t.ID) && t.IsMedium == true).ToList();
            return data.Count;
        }
        private string GetListModuleByClause(int clauseId)
        {
            var dbo = iSOClauseDA.Repository;
            string lst = "";
            var no = 1;
            //Vinh sửa theo yêu cầu của nhung.Them dieu kien where && t.CenterModule.SystemCode == "BUSINESS"
            var data = dbo.ISOIndexModules.Where(t => t.ISOIndexID == clauseId && t.CenterModule.SystemCode.Equals(SystemCode) && !t.IsDelete && t.IsUse).Select(t => t.CenterModule.Name).ToList();
            foreach (var item in data)
            {
                lst += no + ". " + item + "<br/>";
                no++;
            }
            return lst;
        }
        public List<QualityAuditProgramISOIndexItem> GetByPlan(ModelFilter filter, int auditPlanId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            var dbocenter = iSOClauseDA.Repository;
            var data = dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgram.QualityAuditPlanID == auditPlanId
                                                                    && t.QualityAuditProgram.IsAccept && t.QualityAuditProgram.IsApproval)
                .Distinct().OrderByDescending(t => t.ISOIndexID)
                .Filter(filter).ToList();
            List<QualityAuditProgramISOIndexItem> lst = new List<QualityAuditProgramISOIndexItem>();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new QualityAuditProgramISOIndexItem
                    {
                        DepartmentID = (int)(item.HumanDepartmentID == null ? 0 : item.HumanDepartmentID),
                        DepartmentName = item.HumanDepartment == null ? "" : item.HumanDepartment.Name,
                        IsoIndexCode = dbocenter.ISOIndexes.Where(t => t.ID == item.ISOIndexID).Select(i => i.Clause).FirstOrDefault(),
                        IsoIndexName = dbocenter.ISOIndexes.Where(t => t.ID == item.ISOIndexID).Select(i => i.Name).FirstOrDefault(),
                        ID = item.ID,
                        ISOIndexID = item.ISOIndexID,
                        IsAudit = item.AuditAt.HasValue && item.AuditBy.HasValue ? true : false,
                        IsPass = item.IsPass,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        ProgramName = dbo.QualityAuditPrograms.Where(t => t.ID == item.QualityAuditProgramID).Select(i => i.Name).FirstOrDefault(),
                        QualityAuditProgramID = item.QualityAuditProgramID,
                        AuditNote = item.AuditNote,
                        IsObs = item.IsObs,
                        IsMaximum = item.IsMaximum,
                        IsMedium = item.IsMedium,
                        ListModuleName = GetListModuleByClause(item.ISOIndexID),
                    });
                }
            }
            return lst;
        }
        public QualityAuditProgramISOIndexItem GetByID(int id)
        {
            var dbo = iSOClauseDA.Repository;
            var obj = qualityAuditProgramISODA.GetById(id);
            QualityAuditProgramISOIndexItem objView = new QualityAuditProgramISOIndexItem();
            if (obj != null)
            {
                objView.ID = obj.ID;
                objView.ISOIndexID = obj.ISOIndexID;
                objView.QualityAuditProgramID = obj.QualityAuditProgramID;
                objView.IsoIndexCode = dbo.ISOIndexes.Where(t => t.ID == obj.ISOIndexID).Select(i => i.Clause).FirstOrDefault();
                objView.IsoIndexName = dbo.ISOIndexes.Where(t => t.ID == obj.ISOIndexID).Select(i => i.Name).FirstOrDefault();
                objView.IsPass = obj.IsPass;
                objView.AuditAt = obj.AuditAt;
                objView.AuditBy = obj.AuditBy;
                objView.AuditNote = obj.AuditNote;
            }
            return objView;
        }
        public void Update(QualityAuditProgramISOIndexItem objEdit, int userID)
        {
            var obj = qualityAuditProgramISODA.GetById(objEdit.ID);
            if (obj != null)
            {
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = userID;
                obj.AuditAt = objEdit.AuditAt;
                obj.AuditBy = objEdit.AuditBy;
                obj.IsPass = objEdit.IsPass;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = userID;
                obj.AuditNote = objEdit.AuditNote;
            }
            qualityAuditProgramISODA.Update(obj);
            qualityAuditProgramISODA.Save();

        }
        #endregion


        #region Code mới 07/05
        // Lấy danh sách phiếu đánh giá theo chương trình 
        public List<QualityAuditRecordedVoteItem> GetProgramVote(int page, int pageSize, out int total, int programId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            var dbocenter = iSOClauseDA.Repository;
            var query = dbo.QualityAuditRecordedVotes.Where(i => i.QualityAuditProgramID == programId);
            var results = query.Select(item => new QualityAuditRecordedVoteItem()
            {
                ID = item.ID,
                HumanDepartmentName = item.HumanDepartment.Name,
                HumanDepartmentID = item.HumanDepartmentID,
                QualityAuditProgramID = item.QualityAuditProgramID,
                AuditName = dbo.HumanEmployees.Where(e => e.ID == item.AuditBy).Select(e => e.Name).FirstOrDefault(),
                AuditAt = item.AuditAt,
                IsMaximum = item.IsMaximum,
                IsMedium = item.IsMedium,
                IsObs = item.IsObs,
                ISOIndexID = item.ISOIndexID,
                ModuleCode = item.ModuleCode,
                FunctionCode = item.FunctionCode,
                Contents = item.Contents,
                IsComplete = item.IsComplete,
                CreateAt = item.CreateAt
            })
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            foreach (var item in results)
            {
                item.ISOIndexName = dbocenter.ISOIndexes.Where(i => i.ID == item.ISOIndexID).Select(i => i.Name).FirstOrDefault();
                item.ModuleName = dbocenter.CenterModules.Where(i => i.Code == item.ModuleCode).Select(i => i.Name).FirstOrDefault();
                item.FunctionName = dbocenter.CenterFunctions.Where(i => i.Code == item.FunctionCode).Select(i => i.Name).FirstOrDefault();
            }
            return results;
        }
        // Lấy danh sách phòng ban theo chương trình đánh giá
        public List<HumanDepartmentItem> GetDepartmentByProgram(int programId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            return dbo.QualityAuditProgramDepartments.Where(i => i.QualityAuditProgramID == programId)
                        .Select(item => new HumanDepartmentItem()
                        {
                            ID = item.HumanDepartmentID,
                            Name = item.HumanDepartment.Name
                        }).ToList();
        }
        // Lấy phiếu đánh giá
        public QualityAuditRecordedVoteItem GetRecordVote(int id)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            var dbocenter = iSOClauseDA.Repository;
            var obj = dbo.QualityAuditRecordedVotes.Where(i => i.ID == id).FirstOrDefault();
            QualityAuditRecordedVoteItem objView = new QualityAuditRecordedVoteItem();
            if (obj != null)
            {
                objView.ID = id;
                objView.HumanDepartmentID = obj.HumanDepartmentID;
                objView.HumanDepartmentName = obj.HumanDepartment.Name;
                objView.QualityAuditProgramID = obj.QualityAuditProgramID;
                objView.QualityAuditProgramISOID = obj.QualityAuditProgramISOID;
                objView.AuditAt = obj.AuditAt;
                objView.AuditBy = obj.AuditBy;
                objView.ModuleCode = obj.ModuleCode;
                objView.FunctionCode = obj.FunctionCode;
                objView.FunctionName = dbocenter.CenterFunctions.Where(i => i.Code == obj.FunctionCode).Select(i => i.Name).FirstOrDefault();
                objView.ISOIndexID = obj.ISOIndexID;
                objView.Contents = obj.Contents;
                objView.IsMaximum = obj.IsMaximum;
                objView.IsMedium = obj.IsMedium;
                objView.IsObs = obj.IsObs;
                objView.Auditer = new HumanEmployeeSV().GetEmployeeView(obj.AuditBy);
            }
            return objView;
        }
        // Thêm mới phiếu đánh giá
        public void InsertVote(QualityAuditRecordedVoteItem objEdit, int userId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            if (objEdit.ID == 0)
            {
                var obj = new QualityAuditRecordedVote();
                obj.HumanDepartmentID = objEdit.HumanDepartmentID;
                obj.QualityAuditProgramID = objEdit.QualityAuditProgramID;
                obj.QualityAuditProgramISOID = objEdit.QualityAuditProgramISOID == 0 ? null : (int?)objEdit.QualityAuditProgramISOID;
                obj.AuditBy = objEdit.Auditer.ID;
                obj.AuditAt = objEdit.AuditAt;
                obj.ModuleCode = objEdit.ModuleCode;
                obj.FunctionCode = objEdit.FunctionCode;
                obj.ISOIndexID = objEdit.ISOIndexID;
                obj.IsMaximum = objEdit.IsMaximum;
                obj.IsMedium = objEdit.IsMedium;
                obj.IsObs = objEdit.IsObs;
                obj.Contents = objEdit.Contents;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = userId;
                dbo.QualityAuditRecordedVotes.Add(obj);
                dbo.SaveChanges();
            }
        }
        // Cập nhật phiếu đánh giá
        public void UpdateVote(QualityAuditRecordedVoteItem objEdit, int userId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            var obj = dbo.QualityAuditRecordedVotes.Find(objEdit.ID);
            obj.HumanDepartmentID = objEdit.HumanDepartmentID;
            obj.QualityAuditProgramISOID = objEdit.QualityAuditProgramISOID;
            obj.AuditBy = objEdit.Auditer.ID;
            obj.AuditAt = objEdit.AuditAt;
            obj.ModuleCode = objEdit.ModuleCode;
            obj.FunctionCode = objEdit.FunctionCode;
            obj.ISOIndexID = objEdit.ISOIndexID;
            obj.IsMaximum = objEdit.IsMaximum;
            obj.IsMedium = objEdit.IsMedium;
            obj.IsObs = objEdit.IsObs;
            obj.Contents = objEdit.Contents;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            dbo.SaveChanges();
        }
        // Lấy danh sách điểm phát hiện theo phiếu đánh giá
        //public List<QualityAuditRecordedResultItem> GetRecordedResult(int voteId)
        //{
        //    var dbo = qualityAuditProgramISODA.Repository;
        //    var dbocenter = iSOClauseDA.Repository;
        //    var recordResults = dbo.QualityAuditRecordedResults.Where(i => i.QualityAuditRecordedVoteID == voteId).ToList();
        //    var result = recordResults.Select(item => new QualityAuditRecordedResultItem
        //                        {
        //                            ID = item.ID,
        //                            ISOIndexID = item.ISOIndexID,
        //                            ISOIndexName = dbocenter.ISOIndexes.Where(i => i.ID == item.ISOIndexID).Select(i => i.Name).FirstOrDefault(),
        //                            ModuleName = dbocenter.CenterModules.Where(i => i.Code == item.ModuleCode).Select(i => i.Name).FirstOrDefault(),
        //                            ModuleCode = item.ModuleCode,
        //                            FunctionCode = item.FunctionCode,
        //                            FunctionName = dbocenter.CenterFunctions.Where(i => i.Code == item.FunctionCode).Select(i => i.Name).FirstOrDefault(),
        //                            IsMaximum = item.IsMaximum,
        //                            IsMedium = item.IsMedium,
        //                            IsObs = item.IsObs,
        //                            IsVerify = item.IsVerify,
        //                            AuditNote = item.AuditNote,
        //                        }).ToList();
        //    return result;
        //}
        // Xác nhận phiếu đánh giá
        public void VerifyVote(List<int> ids, int userId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            var lstRecordedVotes = dbo.QualityAuditRecordedVotes.Where(i => ids.Contains(i.ID)).ToList();
            var audiprogramIsoes = new List<QualityAuditProgramISO>();
            foreach (var recordedVote in lstRecordedVotes)
            {
                recordedVote.IsComplete = true;
                recordedVote.UpdateAt = DateTime.Now;
                recordedVote.UpdateBy = userId;
                dbo.SaveChanges();
                audiprogramIsoes.Add(new QualityAuditProgramISO()
                {
                    QualityAuditProgramID = recordedVote.QualityAuditProgramID,
                    ISOIndexID = (int)recordedVote.ISOIndexID,
                    HumanDepartmentID = recordedVote.HumanDepartmentID,
                    AuditBy = recordedVote.AuditBy,
                    AuditAt = recordedVote.AuditAt,
                    AuditNote = recordedVote.Contents,
                    IsMaximum = recordedVote.IsMaximum,
                    IsMedium = recordedVote.IsMedium,
                    IsObs = recordedVote.IsObs,
                    IsPass = false,
                });
            }
            //var lstResultUpdate = lstVoteResult.Where(i => i.ID != 0).ToList();
            //foreach (var voteResult in lstResultUpdate)
            //{
            //    var updateRecordedItem = dbo.QualityAuditRecordedResults.Where(i => i.ID == voteResult.ID).FirstOrDefault();
            //    updateRecordedItem.IsVerify = voteResult.IsVerify;
            //    dbo.SaveChanges();
            //    if (updateRecordedItem.IsVerify)
            //        audiprogramIsoes.Add(new QualityAuditProgramISO()
            //                 {
            //                     QualityAuditProgramID = obj.QualityAuditProgramID,
            //                     ISOIndexID = (int)updateRecordedItem.ISOIndexID,
            //                     HumanDepartmentID = objEdit.HumanDepartmentID,
            //                     AuditBy = obj.AuditBy,
            //                     AuditAt = obj.AuditAt,
            //                     AuditNote = obj.Contents,
            //                     IsMaximum = voteResult.IsMaximum,
            //                     IsMedium = voteResult.IsMedium,
            //                     IsObs = voteResult.IsObs,
            //                     IsPass = false,
            //                 });
            //}
            if (audiprogramIsoes.Count > 0)
            {
                dbo.QualityAuditProgramISOes.AddRange(audiprogramIsoes);
                dbo.SaveChanges();
            }
        }
        public void UpdateVerifyVote(QualityAuditRecordedVoteItem objEdit, List<QualityAuditRecordedResultItem> lstVoteResult, int userId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            var obj = dbo.QualityAuditRecordedVotes.Find(objEdit.ID);
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            dbo.SaveChanges();
            var lstResultUpdate = lstVoteResult.Where(i => i.ID != 0).ToList();
            foreach (var voteResult in lstResultUpdate)
            {
                //var updateRecordedItem = dbo.QualityAuditRecordedResults.Where(i => i.ID == voteResult.ID).FirstOrDefault();
                //updateRecordedItem.IsVerify = voteResult.IsVerify;
                dbo.SaveChanges();
            }
        }
        // Xóa phiếu đánh giá
        public void DeleteVote(int id)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            var vote = dbo.QualityAuditRecordedVotes.Where(i => i.ID == id).FirstOrDefault();
            dbo.QualityAuditRecordedVotes.Remove(vote);
            dbo.SaveChanges();
        }
        // Danh sách điều khoản theo chương trình đánh giá
        public object GetIsoIndex(int progamId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            var dbocenter = iSOClauseDA.Repository;
            var isoIndexs = dbo.QualityAuditProgramISOes.Where(i => i.QualityAuditProgramID == progamId)
                                .Select(i => i.ISOIndexID).ToList();
            var result = dbocenter.ISOIndexes.Where(i => isoIndexs.Contains(i.ID))
                                    .Select(item => new
                                    {
                                        ID = item.ID,
                                        Name = item.Name
                                    }).ToList();
            return result;
        }
        // Danh sách module theo điều khoản
        public object GetModuleByProgramId(int programId)
        {
            var dbocenter = iSOClauseDA.Repository;
            var dbo = qualityAuditProgramISODA.Repository;
            var lstISOIndex = dbo.QualityAuditProgramISOes.Where(i => i.QualityAuditProgramID == programId).Select(i => i.ISOIndexID).ToList();
            var data = dbocenter.ISOIndexModules
                .Where(t => lstISOIndex.Contains(t.ISOIndexID) && t.CenterModule.SystemCode.Equals(SystemCode)
                                                               && !t.IsDelete && t.IsUse)
                      .Select(item => new
                      {
                          ID = item.CenterModule.Code,
                          Name = item.CenterModule.Name
                      }
                              )
                     .ToList();
            return data;
        }
        // Danh sách module theo điều khoản
        public object GetModuleByIsoIndex(int isoIndexId)
        {
            var dbocenter = iSOClauseDA.Repository;
            var data = dbocenter.ISOIndexModules
                .Where(t => t.ISOIndexID == isoIndexId && t.CenterModule.SystemCode.Equals(SystemCode)
                     && !t.IsDelete && t.IsUse)
                      .Select(item => new
                      {
                          ID = item.CenterModule.Code,
                          Name = item.CenterModule.Name
                      })
                     .ToList();
            return data;
        }
        // Danh sách chức năng theo điều khoản
        public object GetFunctionByModuleCode(string moduleCode = "")
        {
            var dbocenter = iSOClauseDA.Repository;
            var query = dbocenter.ISOIndexFunctions.Where(t => t.CenterFunction.SystemCode.Contains(SystemCode)
                     && !t.IsDelete && t.IsUse
                     );
            if (!string.IsNullOrWhiteSpace(moduleCode))
            {
                var data = query.Where(t => t.CenterFunction.ModuleCode.Contains(moduleCode))
                       .Select(item => new
                       {
                           ID = item.CenterFunction.Code,
                           Name = item.CenterFunction.Name,
                           ModuleCode = item.CenterFunction.ModuleCode,
                           ModuleName = dbocenter.CenterModules.Where(i => i.Code == item.CenterFunction.ModuleCode)
                                                     .Select(i => i.Name).FirstOrDefault()
                       })
                      .ToList();
                return data;
            }
            return null;
        }
        // Lấy danh sách kết quả đánh giá
        public List<QualityAuditProgramISOIndexItem> GetAuditResult(ModelFilter filter, int planId, int departmentId)
        {
            var dbo = qualityAuditProgramISODA.Repository;
            var auditProgramISOes = dbo.QualityAuditProgramISOes
                        .Where(i => i.QualityAuditProgram.QualityAuditPlanID == planId && i.HumanDepartmentID == departmentId)
                        .Select(item => new QualityAuditProgramISOIndexItem()
                        {
                            ID = item.ID,
                            DepartmentName = item.HumanDepartment.Name,
                            AuditAt = item.AuditAt,
                            AuditBy = item.AuditBy,
                            AuditNote = item.AuditNote,
                            ISOIndexID = item.ISOIndexID,
                            IsMaximum = item.IsMaximum,
                            IsMedium = item.IsMedium,
                            IsObs = item.IsObs,
                            CreateAt = item.CreateAt
                        })
                        .Filter(filter)
                        .ToList();
            var dbocenter = iSOClauseDA.Repository;
            foreach (var programIso in auditProgramISOes)
            {
                programIso.IsoIndexName = dbocenter.ISOIndexes.Where(i => i.ID == programIso.ISOIndexID).Select(i => i.Name).FirstOrDefault();
            }
            return auditProgramISOes;
        }
        #endregion


    }
}
