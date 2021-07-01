using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.Services;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class SuppliersOrderRequirementSV
    {
        private SuppliersOrderRequirementDA suppOrderDA = new SuppliersOrderRequirementDA();
        public List<SuppliersOrderRequirementItem> GetOrderRequirement(ModelFilter filter, int focusId = 0)
        {
            var dbo = suppOrderDA.Repository;
            IQueryable<iDAS.Base.SuppliersOrderRequirement> query = dbo.SuppliersOrderRequirements;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var users = query.Filter(filter)
                        .Select(item => new SuppliersOrderRequirementItem()
                        {
                            ID = item.ID,
                            CreateUser = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanUsers.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).HumanEmployee.Name,
                            },
                            CODE = item.CODE,
                            IsApproval = item.IsApproval.HasValue ? item.IsApproval.Value : false,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,
                            Name = item.Name,
                            CreateUserName = dbo.HumanUsers.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).HumanEmployee.Name
                        })
                        .ToList();
            return users;
        }
        public List<SuppliersOrderRequirementItem> GetOrderRequirement(int page, int pageSize, out int totalCount)
        {
            var dbo = suppOrderDA.Repository;
            var users = dbo.SuppliersOrderRequirements
                        .Select(item => new SuppliersOrderRequirementItem()
                        {
                            ID = item.ID,
                            CreateUser = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanUsers.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).HumanEmployee.Name,
                            },
                            CODE = item.CODE,
                            IsApproval = item.IsApproval.HasValue ? item.IsApproval.Value : false,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,
                            Name = item.Name,
                            CreateUserName = dbo.HumanUsers.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).HumanEmployee.Name
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public List<SuppliersOrderRequirementItem> GetOrderRequirementApproved(int page, int pageSize, out int totalCount)
        {
            var dbo = suppOrderDA.Repository;
            var users = dbo.SuppliersOrderRequirements.Where(i => i.Status == 4 && i.SuppliersOrderRequirementDetails.FirstOrDefault(x => x.Status == 1) != null)
                        .Select(item => new SuppliersOrderRequirementItem()
                        {
                            ID = item.ID,
                            CreateUser = new HumanEmployeeViewItem()
                            {
                                Name = dbo.HumanUsers.FirstOrDefault(
                                i => i.ID == item.CreateBy
                                ).HumanEmployee.Name,
                            },
                            CODE = item.CODE,
                            IsApproval = item.IsApproval.HasValue ? item.IsApproval.Value : false,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Status = item.Status,
                            Name = item.Name
                        })
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        public SuppliersOrderRequirementItem GetById(int id)
        {
            SuppliersOrderRequirementItem obj = new SuppliersOrderRequirementItem();
            var dbo = suppOrderDA.Repository;
            var data = suppOrderDA.Get(t => t.ID == id).FirstOrDefault();
            if (data != null)
            {
                obj.ID = data.ID;
                obj.Name = data.Name;
                obj.Note = data.Note;
                obj.SuppliersOrderRequirementDetails = data.SuppliersOrderRequirementDetails.Select(item => new SuppliersOrderRequirementDetailItem()
                {
                    ID = item.ID,
                    Quantity = item.Quantity,
                    Note = item.Note,
                    StockProduct = new StockProductItem
                    {
                        ID = item.StockProduct.ID,
                        Name = item.StockProduct.Name,
                        Retail_Price = item.StockProduct.Retail_Price,
                        Unit_Name = dbo.StockUnits.FirstOrDefault(i => i.ID == item.StockProduct.Unit_ID).Name,
                        Code = item.StockProduct.Code
                    },
                    Status = item.Status
                }).ToList();
                obj.CODE = data.CODE;
                obj.Approval = new iDAS.Items.HumanEmployeeViewItem()
                {
                    ID = data.ApprovalBy != null ? (int)data.ApprovalBy : 0,
                    Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == data.ApprovalBy).Name,
                    Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == data.ApprovalBy).HumanRole.Name,
                    Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == data.ApprovalBy).HumanRole.HumanDepartment.Name,
                };
                obj.CreateUserID = data.CreateBy != null ? dbo.HumanUsers.Where(m => m.ID == data.CreateBy).Select(i => i.HumanEmployee.ID).FirstOrDefault() : 0;
                obj.CreateUserName = dbo.HumanUsers.Where(m => m.ID == data.CreateBy).Select(i => i.HumanEmployee.Name).FirstOrDefault();
                obj.ApprovalBy = data.ApprovalBy;
                obj.IsEdit = data.IsEdit.HasValue ? data.IsEdit.Value : true;
                obj.IsApproval = data.IsApproval.HasValue ? data.IsApproval.Value : true;
                obj.IsAccept = data.IsAccept.HasValue ? data.IsAccept.Value : false;
                obj.Status = data.Status;
                obj.CreateAt = data.CreateAt;
                obj.CreateBy = data.CreateBy;
                obj.UpdateAt = data.UpdateAt;
                obj.UpdateBy = data.UpdateBy;
            }
            return obj;
        }

        public string GetMaxCode()
        {
            var lstTmp = suppOrderDA.GetQuery(t => t.CODE.Contains("YC")).OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.CODE;
            return string.Empty;
        }


        public int Insert(SuppliersOrderRequirementItem data)
        {
            SuppliersOrderRequirement obj = new SuppliersOrderRequirement();
            if (data != null)
            {
                obj.CODE = data.CODE;
                obj.Name = data.Name;
                obj.Note = data.Note;
                obj.Status = data.Status;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = data.CreateBy;
                obj.ApprovalBy = data.Approval.ID;
                obj.IsEdit = data.IsEdit;
                obj.IsApproval = data.IsApproval;
            }
            suppOrderDA.Insert(obj);
            suppOrderDA.Save();
            return obj.ID;
        }

        public void Update(SuppliersOrderRequirementItem data)
        {
            if (data != null)
            {
                SuppliersOrderRequirement obj = suppOrderDA.GetById(data.ID);
                obj.CODE = data.CODE;
                obj.Name = data.Name;
                obj.Note = data.Note;
                obj.Status = data.Status;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = data.UpdateBy;
                obj.ApprovalBy = data.Approval.ID;
                obj.IsEdit = data.IsEdit;
                obj.IsApproval = data.IsApproval;

                suppOrderDA.Update(obj);
                suppOrderDA.Save();
            }
        }

        public void Delete(int ids)
        {
            new SuppliersOrderRequirementDetailSV().DeleteByIdSuppOrderID(ids);
            suppOrderDA.Delete(ids);
            suppOrderDA.Save();
        }

        public void Approved(SuppliersOrderRequirementItem data)
        {
            var dbo = suppOrderDA.Repository;
            var pl = suppOrderDA.GetById(data.ID);
            pl.IsApproval = true;
            pl.IsEdit = data.IsEdit;
            pl.ApprovalAt = data.ApprovalAt;
            pl.IsAccept = data.IsAccept;
            pl.ApprovalNote = data.ApprovalNote;
            if (data.IsAccept == true)
                pl.Status = 4;
            else if (data.IsEdit == true)
                pl.Status = 2;
            else
                pl.Status = 5;
            suppOrderDA.Update(pl);
            suppOrderDA.Save();
        }
    }
}
