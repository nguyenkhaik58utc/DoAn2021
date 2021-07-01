using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Utilities;

namespace iDAS.Services
{
    public class ProductDevelopmentRequirementPlanSV
    {
        private ProductDevelopmentRequirementPlanDA productDevelopmentRequirementPlanDA = new ProductDevelopmentRequirementPlanDA();
        private QualityPlanDA planDAQ = new QualityPlanDA();
        private QualityPlanSV planServiceQ = new QualityPlanSV();
        public List<DocumentItem> GetDocument(int page, int pageSize, out int totalCount, int productDevelopmentRequirementPlanID)
        {
            var dbo = productDevelopmentRequirementPlanDA.Repository;
            List<DocumentItem> services = new List<DocumentItem>();
            var documentList = dbo.ProductDevelopmentRequirementPlanDocuments.Where(t => t.ProductDevelopmentRequirementPlanID == productDevelopmentRequirementPlanID).Select(t => t.DocumentID).ToArray();
            if (documentList != null && documentList.Count() > 0)
            {
                services = dbo.Documents.Where(t => documentList.Contains(t.ID))
                    .Select(i => new DocumentItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        ParentID = i.ParentID,
                        Code = i.Code,
                        Version = i.Version,
                        IssuedDate = i.PublicDate,
                        IssuedTime = i.PublicNumber,
                        Note = i.Note,
                        FormH = i.FormH,
                        FormS = i.FormS,
                        Security = i.DocumentSecurity.Name,
                        Color = i.DocumentSecurity.Color,
                        EffectiveDate = i.PublicDate,
                        IsEdit = i.IsEdit,
                        Type = i.Type ? "Bên ngoài" : "Nội bộ",
                        TypeID = i.Type ? (int)DocumentType.Out : (int)DocumentType.In,
                        IsApproval = i.IsApproval,
                        IsDeleted = i.IsDelete,
                        IsAccept = i.IsAccept,
                        IsPublic = i.IsPublic,
                        IsObs = i.IsObsolete,
                        HasRequest = false,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                        ApprovalBy = i.ApprovalBy,
                        FlagModified = false
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
                if (services != null && services.Count() > 0)
                {
                    //Lay DS TL Co YC Sua DOi
                    var arReq = dbo.DocumentRequirements.Where(p => p.DocumentID > 0).Select(p => p.DocumentID).ToArray();

                    foreach (var item in services)
                    {
                        int code = 0;
                        if (arReq != null && arReq.Count() > 0 && arReq.Contains(item.ID))
                            item.HasRequest = true;

                        item.Status = getStatus(item.IsEdit, item.IsApproval, item.IsAccept, item.IsPublic, item.IsObs, ref code, item.ParentID, true);
                        item.StatusCode = code;
                        if (item.IsPublic && !item.IsObs && item.ParentID > 0)
                            item.FlagModified = true;
                    }
                }
            }
            else
            {
                services.Page(page, pageSize, out totalCount)
                    .ToList();
            }
            return services;

        }
        public string getStatus(bool isEdit, bool isApp, bool isSucc, bool isPublic, bool isObs, ref int code, int? parentID = 0, bool color = false, bool ckReq = false)
        {

            if (isEdit && !isApp && parentID > 0)
            {
                code = (int)Common.DocumentStatus.New;
                return "Tạo mới";
            }
            else if (isEdit && !isApp)
            {
                code = (int)Common.DocumentStatus.New;
                return "Tạo mới";
            }
            else if (!isEdit && !isApp)
            {
                code = (int)Common.DocumentStatus.PreApprove;
                return "Chờ Duyệt";
            }
            else if (isApp && !isSucc && !isEdit)
            {
                code = (int)Common.DocumentStatus.ApproveFail;
                return "Không duyệt";
            }

            else if (isApp && !isSucc && isEdit)
            {
                code = (int)Common.DocumentStatus.Modified;
                return "Sửa đổi";
            }

            else if (isApp && isSucc && !isPublic)
            {
                code = (int)Common.DocumentStatus.Approve;
                return "Duyệt";
            }

            else if (isPublic && !isObs)
            {

                code = (int)Common.DocumentStatus.Issued;
                if (color)
                    return iDAS.Utilities.Common.StatusColor((int)Utilities.Common.DocumentStatus.Issued);
                if (isPublic && !isObs && !color)
                    return "Ban hành";
            }
            else if (isObs && color)
            {
                code = (int)Common.DocumentStatus.Obsolete;
                return iDAS.Utilities.Common.StatusColor((int)Utilities.Common.DocumentStatus.Obsolete);
            }

            else if (isObs && !color)
            {

                code = (int)Common.DocumentStatus.Obsolete;
                return "Lỗi thời";
            }

            return "";

        }
        public void Insert(int planId, int requirmentId, int userId)
        {
            var obj = new ProductDevelopmentRequirementPlan();
            obj.QualityPlanID = planId;
            obj.ProductDevelopmentRequirementID = requirmentId;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            productDevelopmentRequirementPlanDA.Insert(obj);
            productDevelopmentRequirementPlanDA.Save();
        }
        public void UpdateProductionRequirement(int id, int requirmentId, int userId)
        {
            var obj = productDevelopmentRequirementPlanDA.GetById(id);
            obj.ProductionRequirementID = requirmentId;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            productDevelopmentRequirementPlanDA.Update(obj);
            productDevelopmentRequirementPlanDA.Save();
        }
        public void ApproveFromDevelopmentProduct(ProductDevelopmentRequirementPlanItem PlanApprovalItem)
        {
            planServiceQ.ApproveFromDevelopmentProduct(PlanApprovalItem);
        }
        public void Update(int planId, int requirmentId, int userId)
        {
            var id = planDAQ.GetById(planId).ProductDevelopmentRequirementPlans.FirstOrDefault().ID;
            var obj = productDevelopmentRequirementPlanDA.GetById(id);
            obj.ProductDevelopmentRequirementID = requirmentId;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            productDevelopmentRequirementPlanDA.Update(obj);
            productDevelopmentRequirementPlanDA.Save();

        }
        public ProductDevelopmentRequirementPlanItem GetById(int Id)
        {
            var dbo = productDevelopmentRequirementPlanDA.Repository;
            var PlanItem = dbo.QualityPlans.Where(i => i.ID == Id)
                        .Select(item => new ProductDevelopmentRequirementPlanItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new iDAS.Items.HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.DepartmentID).Name,
                            },
                            ProductDevelopmentRequirementID = item.ProductDevelopmentRequirementPlans.FirstOrDefault().ProductDevelopmentRequirementID.Value,
                            ProductDevelopmentRequirementPlanID = item.ProductDevelopmentRequirementPlans.FirstOrDefault().ID,
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
                            Approval = new iDAS.Items.HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy != null ? (int)item.ApprovalBy : 0,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.HumanDepartment.Name,
                            },
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.HumanEmployees.FirstOrDefault(a => a.ID == dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployeeID).Name,
                            UpdateAt = item.UpdateAt,
                            UpdateByName = dbo.HumanEmployees.FirstOrDefault(a => a.ID == dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployeeID).Name,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            PlanItem.TargetName = dbo.QualityTargets.FirstOrDefault(i => i.ID == PlanItem.TargetID) != null ?
                "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).CompleteAt.ToString() + " " +
                dbo.QualityTargets.FirstOrDefault(i => i.ID == PlanItem.TargetID).Name + iDAS.Utilities.Common.GetTypeName(dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).Type) + dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).Value.ToString() + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).Unit : string.Empty;
            PlanItem.CreateByEmployeeID = PlanItem.CreateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == PlanItem.CreateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == PlanItem.CreateBy).HumanEmployeeID : null;
            PlanItem.UpdateByEmployeeID = PlanItem.UpdateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == PlanItem.UpdateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == PlanItem.UpdateBy).HumanEmployeeID : null;
            return PlanItem;
        }
        private int[] GetListPlanIDByRequirmentID(int requirmentId)
        {
            try
            {
                int[] data = productDevelopmentRequirementPlanDA
                    .GetQuery(t => t.ProductDevelopmentRequirementID == requirmentId)
                    .Select(t => t.QualityPlanID).ToArray();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private int[] GetListPlanIDByRequirmentIDNotify(int requirmentId, int focusId)
        {
            try
            {
                int[] data = productDevelopmentRequirementPlanDA
                    .GetQuery(t => t.ProductDevelopmentRequirementID == requirmentId || (requirmentId==0&&t.QualityPlanID == focusId))
                    .Select(t => t.QualityPlanID).ToArray();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ProductDevelopmentRequirementPlanItem> GetAll(ModelFilter filter, int requirmentId, int focusId=0)
        {
            var dbo = productDevelopmentRequirementPlanDA.Repository;
            int[] lstPlanId = GetListPlanIDByRequirmentIDNotify(requirmentId, focusId);           
            IQueryable<iDAS.Base.QualityPlan> query = dbo.QualityPlans
                        .Where(i => lstPlanId.Contains(i.ID));
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var users = query.Filter(filter).ToList();
            List<ProductDevelopmentRequirementPlanItem> lst = new List<ProductDevelopmentRequirementPlanItem>();
            if (users != null && users.Count > 0)
            {
                foreach (var item in users)
                {
                    lst.Add(new ProductDevelopmentRequirementPlanItem()
                        {
                            ID = item.ID,
                            ProductDevelopmentRequirementPlanID = item.ProductDevelopmentRequirementPlans.FirstOrDefault().ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new iDAS.Items.HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            ParentID = item.ParentID,
                            Type = item.Type,
                            //ServiceName = item.ServicePlans.FirstOrDefault().ServiceCommandContract.CustomerContract.CustomerOrders.FirstOrDefault().Service.Name,
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
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy

                        });
                }
            }
            return lst;
        }
        public decimal GetRateFinish(int idplanQ)
        {
            var dbo = planDAQ.Repository;
            var lstTaskID = dbo.QualityPlanTasks.Where(x => x.QualityPlanID == idplanQ ).Select(x => x.TaskID).ToArray();
            if (dbo.Tasks.Where(s => lstTaskID.Contains(s.ID) && s.IsComplete).Count() != 0)
            {
                decimal rate = (System.Math.Round((decimal)dbo.Tasks.Where(s => lstTaskID.Contains(s.ID) && s.IsComplete).Count() / (decimal)dbo.Tasks.Where(s => lstTaskID.Contains(s.ID)&& !s.IsPause && !s.IsNew).Count(), 2)) * 100;
                return rate;
            }
            else
            {
                return 0;
            }
        }
        public List<ProductDevelopmentRequirementPlanItem> GetIsApproval(int page, int pageSize, out int totalCount, int requirmentId)
        {
            var dbo = productDevelopmentRequirementPlanDA.Repository;
            int[] lstPlanId = GetListPlanIDByRequirmentID(requirmentId);
            var users = dbo.QualityPlans.Where(i => lstPlanId.Contains(i.ID) && i.IsAccept)
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            List<ProductDevelopmentRequirementPlanItem> lst = new List<ProductDevelopmentRequirementPlanItem>();

            if (users != null && users.Count > 0)
            {
                foreach (var item in users)
                {
                    lst.Add(new ProductDevelopmentRequirementPlanItem()
                    {
                        ID = item.ID,
                        ProductDevelopmentRequirementPlanID = item.ProductDevelopmentRequirementPlans.FirstOrDefault().ID,
                        Name = item.Name,
                        TargetID = item.QualityTargetID,
                        Department = new iDAS.Items.HumanDepartmentViewItem()
                        {
                            ID = item.DepartmentID,
                            Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                        },
                        ParentID = item.ParentID,
                        Type = item.Type,
                        ProductionRequirementID = item.ProductDevelopmentRequirementPlans.FirstOrDefault().ProductionRequirementID.HasValue?item.ProductDevelopmentRequirementPlans.FirstOrDefault().ProductionRequirementID:0,
                        Cost = item.Cost,
                        IsPause = item.ProductDevelopmentRequirementPlans.FirstOrDefault().ProductionRequirementID.HasValue ? dbo.ProductionRequirements.FirstOrDefault().IsPause : false,
                        IsFinish = item.ProductDevelopmentRequirementPlans.FirstOrDefault().ProductionRequirementID.HasValue? dbo.ProductionRequirements.FirstOrDefault().IsFinish : false,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        Content = item.Content,
                        IsEdit = item.IsEdit,
                        IsApproval = item.IsApproval,
                        RateFinish = GetRateFinish(item.ID),
                        IsAccept = item.IsAccept,
                        ApprovalAt = item.ApprovalAt,
                        ApprovalBy = item.ApprovalBy,
                        IsDelete = item.IsDelete,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy

                    });
                }
            }
            return lst;
        }
        public void DeleteByPlanID(int planId)
        {
            var obj = productDevelopmentRequirementPlanDA.GetQuery(t => t.QualityPlanID == planId).FirstOrDefault();
            productDevelopmentRequirementPlanDA.Delete(obj);
            productDevelopmentRequirementPlanDA.Save();
        }
        public ProductViewItem GetProductIDByID(int id)
        {
            ProductViewItem obi = new ProductViewItem();
            var obj = productDevelopmentRequirementPlanDA.GetById(id).ProductDevelopmentRequirement;
            if (obj != null)
            {
                obi.ID = obj.StockProductID;
                obi.Name = obj.StockProduct.Name;
            }
            else
            {
                obi.ID = 0;
                obi.Name = string.Empty;
            }
            return obi;
        }
    }
}
