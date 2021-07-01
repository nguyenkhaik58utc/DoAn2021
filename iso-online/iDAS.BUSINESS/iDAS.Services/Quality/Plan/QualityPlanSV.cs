using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;

namespace iDAS.Services
{
    public class QualityPlanSV
    {
        private QualityPlanDA PlanDA = new QualityPlanDA();
        private QualityTargetSV TargetSV = new QualityTargetSV();
        public List<QualityTargetItem> GetAllTarget()
        {
            return TargetSV.GetAll();
        }
        /// <summary>
        /// Lấy tất cả cuộc họp 
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<QualityPlanItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = PlanDA.Repository;
            var users = PlanDA.GetQuery(i => i.IsDelete == false)
                        .Select(item => new QualityPlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            ParentID = item.ParentID,
                            Type = item.Type,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsDelete = item.IsDelete,
                            IsEnd = item.IsEnd,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public List<QualityPlanItem> GetAll()
        {
            var dbo = PlanDA.Repository;
            var users = PlanDA.GetQuery(i => i.IsDelete == false)
                        .Select(item => new QualityPlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            ParentID = item.ParentID,
                            Type = item.Type,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            IsDelete = item.IsDelete,
                            IsEnd = item.IsEnd,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return users;
        }
        /// <summary>
        /// Lấy cuộc họp theo đơn vị
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public List<QualityPlanItem> GetByDepartmentID(int page, int pageSize, out int totalCount, int DepartmentID)
        {
            var dbo = PlanDA.Repository;
            var users = dbo.QualityPlans.Where(i => i.IsDelete == false && i.DepartmentID == DepartmentID)
                        .Select(item => new QualityPlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            ParentID = item.ParentID,
                            Type = item.Type,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsApproval = item.IsApproval,
                            IsEdit = item.IsEdit,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            IsDelete = item.IsDelete,
                            IsEnd = item.IsEnd,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// Lấy cuộc họp  theo mã cuộc họp
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public QualityPlanItem GetById(int Id)
        {
            var dbo = PlanDA.Repository;
            var PlanItem = dbo.QualityPlans.Where(i => i.ID == Id)
                        .Select(item => new QualityPlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID.HasValue ? item.QualityTargetID : 0,
                            //TargetName = item.QualityTargetID.HasValue ? "Đến ngày: "
                            //+ item.QualityTarget.Date.ToString()
                            //+ " " + item.QualityTarget.QualityTargetType.Name + iDAS.Utilities.Common.GetFormName(item.QualityTarget.Form.Value) + item.QualityTarget.Number + " " + item.QualityTarget.Unit : string.Empty,
                            Department = new HumanDepartmentViewItem()
                              {
                                  ID = item.DepartmentID,
                                  Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                              },
                            ParentID = item.ParentID,
                            ParentName = dbo.QualityPlans.FirstOrDefault(p => p.ID == item.ParentID).Name,
                            Type = item.Type,
                            ApprovalNote = item.Note,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsAccept = item.IsAccept,
                            IsEdit = item.IsEdit,
                            IsDelete = item.IsDelete,
                            Approval = new HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy != null ? (int)item.ApprovalBy : 0,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.HumanDepartment.Name,
                            },
                            IsEnd = item.IsEnd,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                            UpdateAt = item.UpdateAt,
                            UpdateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            var targetPlan = dbo.QualityTargets.FirstOrDefault(i => i.ID == PlanItem.TargetID);
            PlanItem.TargetName = targetPlan == null ? string.Empty : ("Đến ngày: "
                            + targetPlan.CompleteAt.ToString()
                            + " " + iDAS.Utilities.Common.GetTypeName(targetPlan.Type) + targetPlan.Value + " " + targetPlan.Unit);
            return PlanItem;
        }
        /// <summary>
        /// Gửi bản ghi cho lãnh đạo phê duyệt IsEdit = false; Lãnh đạo trả về để sửa IsSend = false
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="userID"></param>
        public void SendApproval(QualityPlanItem item, int userID)
        {
            var rp = PlanDA.GetById(item.ID);
            rp.ApprovalBy = item.ApprovalBy;
            rp.IsEdit = item.IsEdit;
            rp.IsApproval = item.IsApproval;
            PlanDA.Save();
        }
        /// <summary>
        ///Phê duyệt cuộc họp 
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="ID">Mã cuộc họp</param>
        /// <param name="UserID"> Người phê duyệt</param>
        /// <param name="note"> Ghi chú phê duyệt</param>
        public void Approve(QualityPlanItem PlanApprovalItem)
        {
            var pl = PlanDA.GetById(PlanApprovalItem.ID);
            pl.IsApproval = true;
            pl.IsEdit = PlanApprovalItem.IsEdit;
            pl.ApprovalAt = PlanApprovalItem.ApprovalAt;
            pl.IsAccept = PlanApprovalItem.IsAccept;
            pl.Note = PlanApprovalItem.ApprovalNote;
            PlanDA.Save();
        }
        /// <summary>

        /// || Author: cuongpc 
        /// </summary>
        /// <param name="ID">Mã cuộc họp</param>
        /// <param name="UserID"> Người phê duyệt</param>
        /// <param name="note"> Ghi chú phê duyệt</param>
        public void ApproveFromService(ServicePlanItem PlanApprovalItem)
        {
            var dbo = PlanDA.Repository;
            var pl = PlanDA.GetById(PlanApprovalItem.ID);
            pl.IsApproval = true;
            pl.IsEdit = PlanApprovalItem.IsEdit;
            pl.ApprovalAt = PlanApprovalItem.ApprovalAt;
            pl.IsAccept = PlanApprovalItem.IsAccept;
            pl.Note = PlanApprovalItem.ApprovalNote;
            if (PlanApprovalItem.IsAccept)
            {
                var lst = PlanDA.GetQuery(t => t.ParentID == PlanApprovalItem.ID).ToList();
                foreach (var item in lst)
                {
                    item.IsApproval = true;
                    item.IsEdit = PlanApprovalItem.IsEdit;
                    item.ApprovalAt = PlanApprovalItem.ApprovalAt;
                    item.IsAccept = PlanApprovalItem.IsAccept;
                    item.Note = PlanApprovalItem.ApprovalNote;
                    PlanDA.Update(item);
                }
                var obj = dbo.CustomerContracts.FirstOrDefault(t => t.ID == dbo.ServiceCommandContracts.FirstOrDefault(s => s.ID == PlanApprovalItem.ServiceCommandContractID).CustomerContractID);
                obj.IsStart = true;
            }
            PlanDA.Save();
        }
        /// <summary>

        /// || Author: cuongpc 
        /// </summary>
        /// <param name="ID">Mã cuộc họp</param>
        /// <param name="UserID"> Người phê duyệt</param>
        /// <param name="note"> Ghi chú phê duyệt</param>
        public void ApproveFromDevelopmentProduct(ProductDevelopmentRequirementPlanItem PlanApprovalItem)
        {
            var dbo = PlanDA.Repository;
            var pl = PlanDA.GetById(PlanApprovalItem.ID);
            pl.IsApproval = true;
            pl.IsEdit = PlanApprovalItem.IsEdit;
            pl.ApprovalAt = PlanApprovalItem.ApprovalAt;
            pl.IsAccept = PlanApprovalItem.IsAccept;
            pl.Note = PlanApprovalItem.ApprovalNote;
            if (PlanApprovalItem.IsAccept)
            {
                var requirment = dbo.ProductDevelopmentRequirements.Where(t => t.ID == PlanApprovalItem.ProductDevelopmentRequirementID).FirstOrDefault();
                requirment.IsWork = true;
                dbo.SaveChanges();
            }
            PlanDA.Save();
        }
        /// <summary>
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// Cập nhật cuộc họp 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(QualityPlanItem item, int userID)
        {
            var pI = PlanDA.GetById(item.ID);
            pI.Name = item.Name;
            pI.QualityTargetID = item.TargetID;
            pI.DepartmentID = item.Department.ID;
            pI.ParentID = item.ParentID;
            pI.Type = item.Type;
            pI.Content = item.Content;
            pI.Cost = item.Cost;
            pI.StartTime = item.StartTime;
            pI.EndTime = item.EndTime;
            pI.IsApproval = item.IsApproval;
            pI.IsEdit = item.IsEdit;
            pI.IsAccept = item.IsAccept;
            pI.ApprovalAt = item.ApprovalAt;
            pI.ApprovalBy = item.ApprovalBy;
            pI.Note = item.ApprovalNote;
            pI.UpdateAt = DateTime.Now;
            pI.UpdateBy = userID;
            PlanDA.Update(pI);
            PlanDA.Save();
        }
        /// <summary>
        /// Cập nhật các thông tin cơ bản của cuộc họp 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void UpdateInfo(QualityPlanItem item, int userID)
        {
            var pI = PlanDA.GetById(item.ID);
            pI.Name = item.Name;
            pI.QualityTargetID = item.TargetID;
            pI.DepartmentID = item.Department.ID;
            pI.ParentID = item.ParentID;
            pI.Type = item.Type;
            pI.StartTime = item.StartTime;
            pI.EndTime = item.EndTime;
            pI.Cost = item.Cost;
            pI.Content = item.Content;
            pI.UpdateAt = DateTime.Now;
            pI.UpdateBy = userID;
            PlanDA.Save();
        }
        /// <summary>
        /// Thêm mới cuộc họp 
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(QualityPlanItem item, int userID)
        {
            var plan = new QualityPlan()
            {
                Name = item.Name,
                QualityTargetID = item.TargetID,
                DepartmentID = item.Department.ID,
                ParentID = item.ParentID,
                Type = item.Type,
                Note = item.ApprovalNote,
                Cost = item.Cost,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Content = item.Content,
                // Thêm mới mặc đinh là chưa xóa và cho phép sửa
                IsEdit = item.IsEdit,
                // Nội dung phê duyệt
                IsAccept = item.IsAccept,
                IsApproval = item.IsApproval,
                ApprovalBy = item.ApprovalBy,
                // Thông tin hệ thống
                CreateAt = DateTime.Now,
                CreateBy = userID,

            };
            PlanDA.Insert(plan);
            PlanDA.Save();
        }
        public int Insert(QualityPlanItem item)
        {
            var dbo = PlanDA.Repository;
            QualityPlan obj = new QualityPlan();
            obj.Name = item.Name;
            obj.QualityTargetID = item.TargetID;
            obj.DepartmentID = item.Department.ID;
            obj.ParentID = item.ParentID.HasValue ? item.ParentID : 0;
            obj.Type = item.Type;
            obj.Note = item.ApprovalNote;
            obj.Cost = item.Cost;
            obj.StartTime = item.StartTime;
            obj.EndTime = item.EndTime;
            if (item.Approval == null)
            {
                obj.ApprovalBy = dbo.QualityPlans.FirstOrDefault(t => t.ID == item.ParentID).ApprovalBy;
            }
            else
            {
                obj.ApprovalBy = item.Approval.ID == 0 ? dbo.QualityPlans.FirstOrDefault(t => t.ID == item.ParentID).ApprovalBy : item.Approval.ID;
            }
            obj.Content = item.Content;
            // Thêm mới mặc đinh là chưa xóa và cho phép sửa
            obj.IsEdit = item.IsEdit;
            obj.IsApproval = item.IsApproval;
            obj.IsAccept = false;
            obj.IsDelete = false;
            obj.CreateAt = item.CreateAt;
            obj.CreateBy = item.CreateBy;
            PlanDA.Insert(obj);
            PlanDA.Save();
            return obj.ID;
        }
        /// <summary>
        /// Xóa cuộc họp 
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            PlanDA.Delete(id);
            PlanDA.Save();
        }
        /// <summary>
        /// Xóa tạm cuộc họp  khi cuộc họp đang được sử dụng ở bản ghi khác
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="id"></param>
        public void isDelete(int id)
        {
            var rp = PlanDA.GetById(id);
            rp.IsDelete = true;
            PlanDA.Save();
        }
        public QualityPlanItem GetPlanEnd(int id)
        {
            return PlanDA.GetQuery(i => i.ID == id)
                 .Select(item => new QualityPlanItem()
                 {
                     ID = item.ID,
                     IsEnd = item.IsEnd,
                     EndAt = item.EndAt,
                 }).FirstOrDefault();
        }
        public void End(QualityPlanItem item, int userId)
        {
            var endItem = PlanDA.GetById(item.ID);
            endItem.EndAt = item.EndAt;
            endItem.IsEnd = item.IsEnd;
            endItem.UpdateBy = userId;
            endItem.UpdateAt = DateTime.Now;
            PlanDA.Save();
        }
        public List<QualityPlanItem> GetChildPlan(int parentId, bool isRoot)
        {
            var dbo = PlanDA.Repository;
            return dbo.QualityPlans
                      .Where(i => (i.ID == parentId && isRoot) || (i.ParentID == parentId && !isRoot))
                      .Select(
                                item => new QualityPlanItem()
                                    {
                                        ID = item.ID,
                                        Name = item.Name,
                                        IsEdit = item.IsEdit,
                                        IsApproval = item.IsApproval,
                                        IsAccept = item.IsAccept,
                                        Leaf = !dbo.QualityPlans.Any(i => i.ParentID == item.ID)
                                    }
                            ).ToList();
        }
        public bool CheckExits(string name)
        {
            var dbo = PlanDA.Repository;
            var obj = PlanDA.GetQuery(t => t.Name.Trim().ToUpper().Equals(name.Trim().ToUpper())).FirstOrDefault();
            if (obj != null)
                return true;
            return false;
        }
        public bool CheckEditExits(int id,string name)
        {
            var dbo = PlanDA.Repository;
            var obj = PlanDA.GetQuery(t => t.Name.Trim().ToUpper().Equals(name.Trim().ToUpper())&& t.ID!=id).FirstOrDefault();
            if (obj != null)
                return true;
            return false;
        }
    }
}
