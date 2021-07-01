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
    public class QualityCAPASV
    {
        private QualityCAPADA CAPADA = new QualityCAPADA();
        public QualityCAPAItem GetByID(int id)
        {
            var CAPA = CAPADA.GetQuery()
                .Where(i => i.ID == id)
                .Select(item => new QualityCAPAItem()
                {
                    ID = item.ID,
                    RequireID = item.QualityCAPARequirementID,
                    IsEdit = item.IsEdit,
                    IsApproval = item.IsApproval,
                    IsAccept = item.IsAccept,
                    IsComplete = item.IsComplete,
                    IsPass = item.IsPass,
                    CompleteTime = item.CompleteTime,
                    CompleteRealTime = item.CompleteRealTime,
                    Cause = item.Cause,
                    Solution = item.Solution,
                    ApprovalBy = item.ApprovalBy,
                    ApprovalAt = item.ApprovalAt,
                    ApprovalNote = item.ApprovalNote,
                })
                .First();

            return CAPA;
        }
        public QualityCAPAItem GetByRequireId(int requireId)
        {
            var dbo = CAPADA.Repository;
            var CAPA = dbo.QualityCAPARequirements.Where(i => i.ID == requireId)
                            .Select(temp => new
                            {
                                CAPA = temp.QualityCAPAs.FirstOrDefault(),
                                //--Thông tin yêu cầu khắc phục phòng ngừa
                                Name = temp.Name,
                                Code = temp.Code,
                                Represent = new HumanEmployeeViewItem()
                                {
                                    ID = temp.EmployeeID == null ? 0 : (int)temp.EmployeeID,
                                    Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == temp.EmployeeID).Name,
                                    Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == temp.EmployeeID).HumanRole.Name,
                                    Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == temp.EmployeeID).HumanRole.HumanDepartment.Name,

                                },
                                //CheckAt = temp.CheckAt,
                                //CheckNote = temp.CheckNote,
                                //-------------------------------
                            })
                            .Select(
                               p => new QualityCAPAItem()
                                    {
                                        ID = p.CAPA != null ? p.CAPA.ID : 0,
                                        RequireID = requireId,
                                        IsEdit = p.CAPA == null ? true : p.CAPA.IsEdit,
                                        IsApproval = p.CAPA == null ? false : p.CAPA.IsApproval,
                                        IsAccept = p.CAPA == null ? false : p.CAPA.IsAccept,
                                        IsComplete = p.CAPA == null ? false : p.CAPA.IsComplete,
                                        IsPass = p.CAPA == null ? false : p.CAPA.IsPass,
                                        CompleteTime = p.CAPA == null ? null : p.CAPA.CompleteTime,
                                        CompleteRealTime = p.CAPA == null ? null : p.CAPA.CompleteRealTime,
                                        Cause = p.CAPA == null ? null : p.CAPA.Cause,
                                        Solution = p.CAPA == null ? null : p.CAPA.Solution,
                                        ApprovalBy = p.CAPA == null ? null : p.CAPA.ApprovalBy,
                                        Approval = p.CAPA == null ? null : new HumanEmployeeViewItem()
                                            {
                                                ID = p.CAPA.ApprovalBy == null ? 0 : (int)p.CAPA.ApprovalBy,
                                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == p.CAPA.ApprovalBy).Name,
                                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == p.CAPA.ApprovalBy).HumanRole.Name,
                                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == p.CAPA.ApprovalBy).HumanRole.HumanDepartment.Name,
                                            },
                                        ApprovalAt = p.CAPA == null ? null : p.CAPA.ApprovalAt,
                                        ApprovalNote = p.CAPA == null ? null : p.CAPA.ApprovalNote,
                                        Name = p.Name,
                                        Code = p.Code,
                                        CheckBy = p.Represent,
                                        //CheckAt = p.CheckAt,
                                        //CheckNote = p.CheckNote,
                                    }
                            )
                            .First();
            return CAPA;
        }
        public int Insert(QualityCAPAItem item)
        {
            var CAPA = new QualityCAPA()
            {
                QualityCAPARequirementID = item.RequireID,
                IsEdit = item.IsEdit,
                IsApproval = item.IsApproval,
                IsAccept = item.IsAccept,
                IsComplete = item.IsComplete,
                IsPass = item.IsPass,
                CompleteTime = item.CompleteTime,
                CompleteRealTime = item.CompleteRealTime,
                Cause = item.Cause,
                Solution = item.Solution,
                ApprovalBy = item.ApprovalBy,
                ApprovalNote = item.ApprovalNote,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            CAPADA.Insert(CAPA);
            CAPADA.Save();
            return item.ID;
        }
        public void Update(QualityCAPAItem item)
        {
            var CAPA = CAPADA.GetById(item.ID);
            CAPA.IsEdit = item.IsEdit;
            CAPA.IsApproval = item.IsApproval;
            CAPA.IsAccept = item.IsAccept;
            CAPA.IsComplete = item.IsComplete;
            CAPA.IsPass = item.IsPass;
            CAPA.CompleteTime = item.CompleteTime;
            CAPA.CompleteRealTime = item.CompleteRealTime;
            CAPA.Cause = item.Cause;
            CAPA.Solution = item.Solution;
            CAPA.ApprovalBy = item.ApprovalBy;
            CAPA.UpdateAt = DateTime.Now;
            CAPA.UpdateBy = CAPA.UpdateBy;
            CAPADA.Save();
        }
        public void Approve(QualityCAPAItem item)
        {
            var CAPA = CAPADA.GetById(item.ID);
            CAPA.IsEdit = item.IsEdit;
            CAPA.IsAccept = item.IsAccept;
            CAPA.IsApproval = item.IsApproval;
            CAPA.ApprovalNote = item.ApprovalNote;
            CAPA.ApprovalAt = item.ApprovalAt;
            CAPA.UpdateAt = DateTime.Now;
            CAPA.UpdateBy = CAPA.UpdateBy;
            CAPADA.Save();
        }
        public void Delete(int id)
        {
            CAPADA.Delete(id);
            CAPADA.Save();
        }
    }
}
