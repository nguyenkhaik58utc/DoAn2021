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
    public class QualityAuditDepartmentSV
    {
        private HumanDepartmentDA departmentDA = new HumanDepartmentDA();
        QualityAuditDepartmentDA auditDepartmentDA = new QualityAuditDepartmentDA();
        private int[] GetByAuditPlanID(int auditProgramId)
        {
            try
            {
                var modules = auditDepartmentDA.GetQuery()
                                    .Where(a => a.QualityAuditProgramID == auditProgramId)
                                    .Select(a => a.HumanDepartmentID).ToArray();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool SetIsSelected(int auditProgramId, int departmentID)
        {
            if (GetByAuditPlanID(auditProgramId).Contains(departmentID))
            {
                return true;
            }
            return false;
        }
        public List<QualityAuditDepartmentItem> GetAll(int page, int pageSize, out int total, int auditProgramId)
        {
            var dbo = auditDepartmentDA.Repository;
            List<QualityAuditDepartmentItem> lst = new List<QualityAuditDepartmentItem>();
            var isos = auditDepartmentDA.GetQuery(t => t.QualityAuditProgramID == auditProgramId).OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (isos.Count > 0)
            {
                foreach (var item in isos)
                {
                    lst.Add(new QualityAuditDepartmentItem
                    {
                        ID = item.HumanDepartmentID,
                        Code = dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID) != null ? dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Code : string.Empty,
                        Name = dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID) != null ? dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name : string.Empty,
                        CreateAt = dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID) != null ? dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).CreateAt : DateTime.Now
                    });
                }
            }
            return lst;
        }
        public List<ComboboxItem> GetCombobox(int auditProgramId)
        {
            var dbo = auditDepartmentDA.Repository;
            var data = auditDepartmentDA.GetQuery(t => t.QualityAuditProgramID == auditProgramId).Select(item => new ComboboxItem
            {
                ID = item.HumanDepartmentID,
                Name = item.HumanDepartment.Name
            }).ToList();
            return data;

        }
        public int GetChildrenCount(int parentId, bool viewCancel = false, bool viewDeleted = false)
        {
            var count = departmentDA.GetQuery(t => t.ParentID == parentId)
                            .Where(i => viewCancel || !viewCancel && !i.IsCancel)
                            .Where(i => viewDeleted || !viewDeleted && !i.IsDelete)
                           .Count();

            return count;
        }
        public List<QualityAuditDepartmentItem> GetGroupsByParentID(int parentId, bool viewCancel = false, bool viewDeleted = false, int auditProgramId = 0)
        {
            List<QualityAuditDepartmentItem> lst = new List<QualityAuditDepartmentItem>();
            var groups = departmentDA.GetQuery(t => t.ParentID == parentId)
                            .Where(i => viewCancel || !viewCancel && !i.IsCancel)
                            .Where(i => viewDeleted || !viewDeleted && !i.IsDelete).OrderByDescending(t => t.CreateAt)
                          .ToList();
            foreach (var item in groups)
            {
                lst.Add(new QualityAuditDepartmentItem
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               Name = item.Name,
                               IsDelete = item.IsDelete,
                               IsActive = item.IsActive,
                               IsCancel = item.IsCancel,
                               IsSelected = SetIsSelected(auditProgramId, item.ID)
                           });

            }
            return lst;
        }
        public void InsertRange(List<QualityAuditDepartmentItem> items, int userID)
        {
            var rprs = new List<QualityAuditProgramDepartment>();
            rprs = items.Select(item => new QualityAuditProgramDepartment()
            {
                QualityAuditProgramID = item.AuditProgramID,
                HumanDepartmentID = item.ID

            }).ToList();
            auditDepartmentDA.InsertRange(rprs);
            auditDepartmentDA.Save();
        }
        public bool CheckExitDepartment(int programId, string listDepartmentId, string arrIdDepartRemoves)
        {
            var dbo = auditDepartmentDA.Repository;
            List<string> added = new List<string>();
            List<string> removed = new List<string>();
            var exit = dbo.QualityAuditProgramDepartments
                .Where(t => t.QualityAuditProgramID == programId)
                .Select(t => t.HumanDepartmentID.ToString())
                .ToList();
            if (listDepartmentId != null && !string.IsNullOrEmpty(listDepartmentId.Trim()))
            {
                added = listDepartmentId.Substring(0, listDepartmentId.Length - 1).Split(',').ToList();
            }
            if (arrIdDepartRemoves != null && !string.IsNullOrEmpty(arrIdDepartRemoves.Trim()))
            {
                removed = arrIdDepartRemoves.Substring(0, arrIdDepartRemoves.Length - 1).Split(',').ToList();
            }
            if (added.Count > 0)
            {
                foreach (var item in added)
                {
                    exit.Add(item);
                }
            }
            if (removed.Count > 0)
            {
                foreach (var item in removed)
                {
                    exit.Remove(item);
                }
            }
            if (exit.Count() > 0)
                return true;
            return false;

        }
        public void UpdateRange(string listDepartmentId, string arrIdDepartRemoves, int programId, int userID)
        {
            var dbo = auditDepartmentDA.Repository;
            if (arrIdDepartRemoves != null && arrIdDepartRemoves.Length > 0)
            {
                var departmentRemove = arrIdDepartRemoves.Substring(0, arrIdDepartRemoves.Length - 1).Split(',');
                dbo.QualityAuditProgramDepartments.RemoveRange(dbo.QualityAuditProgramDepartments.Where(t => t.QualityAuditProgramID == programId && arrIdDepartRemoves.Contains(t.HumanDepartmentID.ToString())).ToList());
                dbo.SaveChanges();
            }
            if (listDepartmentId != null && listDepartmentId.Length > 0)
            {
                var arrIdDeparts = listDepartmentId.Substring(0, listDepartmentId.Length - 1).Split(',');
                var rprs = new List<QualityAuditProgramDepartment>();
                foreach (var arr in arrIdDeparts)
                {
                    if (!dbo.QualityAuditProgramDepartments.Any(t => t.QualityAuditProgramID == programId && t.HumanDepartmentID.ToString() == arr))
                    {
                        var rpr = new QualityAuditProgramDepartment();
                        rpr.HumanDepartmentID = int.Parse(arr);
                        rpr.QualityAuditProgramID = programId;
                        rpr.CreateAt = DateTime.Now;
                        rpr.CreateBy = userID;
                        rprs.Add(rpr);
                    }
                }
                auditDepartmentDA.InsertRange(rprs);
                auditDepartmentDA.Save();
            }
        }

    }
}
