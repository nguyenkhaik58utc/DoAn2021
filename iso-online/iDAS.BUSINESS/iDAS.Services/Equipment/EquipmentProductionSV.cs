using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class EquipmentProductionSV
    {
        private EquipmentProductionDA ProductionDA = new EquipmentProductionDA();
        private EquipmentMeasureCategoryDA equipmentMeasureCategoryDA = new EquipmentMeasureCategoryDA();
        private EquipmentProductionMaintainDA EquipmentProductionMaintainDA = new EquipmentProductionMaintainDA();
        private EquipmentMeasureDA equipmentMeasureDA = new EquipmentMeasureDA();
        private IEnumerable<EquipmentProductionCategory> getChilds(IEnumerable<EquipmentProductionCategory> e, int? id)
        {
            var EquipmentProductionCategory = e.Where(a => a.ParentID == id);
            var EquipmentProductionCategoryFirst = e.Where(a => a.ID == id);
            EquipmentProductionCategoryFirst.Concat(EquipmentProductionCategory);
            return EquipmentProductionCategoryFirst.Concat(EquipmentProductionCategory.SelectMany(a => getChilds(e, a.ID)));
        }
        public List<EquipmentProductionItem> GetByCategory(int page, int pageSize, out int totalCount, int categoryId)
        {
            var SelectedCategory = ProductionDA.Repository.EquipmentProductionCategories.Where(i => i.ID == categoryId);
            var result = getChilds(SelectedCategory, categoryId)
                        .SelectMany(i => i.EquipmentProductions)
                        .Select(item => new EquipmentProductionItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            SerialNumber = item.SerialNumber,
                            UseStartTime = item.UseStartTime,
                            MadeIn = item.MadeIn,
                            MadeYear = item.MadeYear,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            IsError = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                            IsFixed = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                            IsMaintain = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                            IsUsing = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }

        public EquipmentProductionItem GetById(int Id)
        {
            var dbo = ProductionDA.Repository;
            var result = ProductionDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentProductionItem()
                        {
                            ID = item.ID,
                            EquipmentProductionCategoryID = item.EquipmentProductionCategoryID,
                            Code = item.Code,
                            Name = item.Name,
                            SerialNumber = item.SerialNumber,
                            UseStartTime = item.UseStartTime,
                            MadeIn = item.MadeIn,
                            MadeYear = item.MadeYear,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            IsError = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                            IsFixed = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                            IsMaintain = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                            IsUsing = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                            Specifications = item.Specifications,
                            SupportInfomation = item.SupportInfomation,
                            Features = item.Features,
                            MaintainContent = item.MaintainContent,
                            MaintainOther = item.MaintainOther,
                            MaintainPeriodic = item.MaintainPeriodic,
                        })
                        .First();
            /*if (result != null)
            {
                var files = dbo.EquipmentProductionAttachmentFiles.Where(i => i.EquipmentProductionID == Id)
                        .Select(i => i.AttachmentFileID).ToList();
                result.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = files
                };
            }*/
            return result;
        }
        /// <summary>
        /// Cập nhật  thiết bị sản xuất
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentProductionItem item, int userID)
        {
            var production = ProductionDA.GetById(item.ID);
            production.ID = item.ID;
            production.EquipmentProductionCategoryID = item.EquipmentProductionCategoryID;
            production.Code = item.Code;
            production.Name = item.Name;
            production.SerialNumber = item.SerialNumber;
            production.UseStartTime = item.UseStartTime;
            production.MadeIn = item.MadeIn;
            production.MadeYear = item.MadeYear;
            production.IsActive = item.IsActive;
            production.IsDelete = item.IsDelete;
            production.IsError = item.IsError;
            production.IsFixed = item.IsFixed;
            production.IsMaintain = item.IsMaintain;
            production.IsUsing = item.IsUsing;
            production.Specifications = item.Specifications;
            production.SupportInfomation = item.SupportInfomation;
            production.Features = item.Features;
            production.MaintainContent = item.MaintainContent;
            production.MaintainOther = item.MaintainOther;
            production.MaintainPeriodic = item.MaintainPeriodic;
            production.UpdateAt = DateTime.Now;
            production.UpdateBy = userID;
            ProductionDA.Save();
            new FileSV().Upload<EquipmentProductionAttachmentFile>(item.AttachmentFiles, item.ID);
        }
        /// <summary>
        /// Thêm mới  thiết bị sản xuất
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentProductionItem item, int userID)
        {
            var EquipmentProduction = new EquipmentProduction()
            {
                Code = item.Code,
                Name = item.Name,
                SerialNumber = item.SerialNumber,
                EquipmentProductionCategoryID = item.EquipmentProductionCategoryID,
                UseStartTime = item.UseStartTime,
                MadeIn = item.MadeIn,
                MadeYear = item.MadeYear,
                IsActive = item.IsActive,
                IsDelete = item.IsDelete,
                IsError = item.IsError,
                IsFixed = item.IsFixed,
                IsMaintain = item.IsMaintain,
                IsUsing = item.IsUsing,
                Specifications = item.Specifications,
                SupportInfomation = item.SupportInfomation,
                Features = item.Features,
                MaintainContent = item.MaintainContent,
                MaintainOther = item.MaintainOther,
                MaintainPeriodic = item.MaintainPeriodic,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ProductionDA.Insert(EquipmentProduction);
            ProductionDA.Save();
            new FileSV().Upload<EquipmentProductionAttachmentFile>(item.AttachmentFiles, EquipmentProduction.ID);
            return EquipmentProduction.ID;
        }
        /// <summary>
        /// Xóa  thiết bị sản xuất
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionDA.Delete(id);
            ProductionDA.Save();
        }

        public List<EquipmentProductionCategoryItem> GetCategory()
        {

            return new EquipmentProductionCategorySV().GetAll();
        }

        public List<EquipmentProductionItem> GetByCategory(ModelFilter filter, int categoryId, DateTime fromDate, DateTime toDate)
        {

            var result = ProductionDA.GetQuery(i => i.EquipmentProductionCategoryID == categoryId && i.UseStartTime <= toDate)
                        .Select(item => new EquipmentProductionItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            SerialNumber = item.SerialNumber,
                            UseStartTime = item.UseStartTime,
                            MadeIn = item.MadeIn,
                            MadeYear = item.MadeYear,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            IsError = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                            IsFixed = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                            IsMaintain = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                            IsUsing = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            return result;
        }

        public List<EquipmentProductionItem> GetTotalUse(ModelFilter filter, int cateId, DateTime searchFromDate, DateTime searchToDate)
        {
            //Thiet bi phan phoi
            //Tong so
            var dbo = ProductionDA.Repository;
            var result = dbo.EquipmentProductionDepartments
                            .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                            .Where(i => i.IsUsing == true)//Thu hồi using = false
                             .Where(i => (i.StartTime <= searchToDate))
                            .Select(item => new EquipmentProductionItem()
                            {
                                ID = item.EquipmentProduction.ID,
                                Code = item.EquipmentProduction.Code,
                                Name = item.EquipmentProduction.Name,
                                SerialNumber = item.EquipmentProduction.SerialNumber,
                                UseStartTime = item.EquipmentProduction.UseStartTime,
                                MadeIn = item.EquipmentProduction.MadeIn,
                                MadeYear = item.EquipmentProduction.MadeYear,
                                IsActive = item.EquipmentProduction.IsActive,
                                IsDelete = item.EquipmentProduction.IsDelete,
                                IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                CreateAt = item.EquipmentProduction.CreateAt
                            })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            return result;
        }

        public List<EquipmentProductionItem> GetUseCKP(ModelFilter filter, int cateId, DateTime searchFromDate, DateTime searchToDate)
        {
            var dbo = ProductionDA.Repository;
            var lstEQDept = dbo.EquipmentProductionDepartments
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                 .Where(i => (i.StartTime <= searchToDate))
                                .Select(i => i.EquipmentProductionID)
                                .ToList();
            var result = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept.Contains(i.EquipmentProductionID) && i.IsFixed == false)
                                .Where(i => i.Time <= searchToDate && i.Time >= searchFromDate)
                            .Select(item => new EquipmentProductionItem()
                            {
                                ID = item.EquipmentProduction.ID,
                                Code = item.EquipmentProduction.Code,
                                Name = item.EquipmentProduction.Name,
                                SerialNumber = item.EquipmentProduction.SerialNumber,
                                UseStartTime = item.EquipmentProduction.UseStartTime,
                                MadeIn = item.EquipmentProduction.MadeIn,
                                MadeYear = item.EquipmentProduction.MadeYear,
                                IsActive = item.EquipmentProduction.IsActive,
                                IsDelete = item.EquipmentProduction.IsDelete,
                                IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                CreateAt = item.EquipmentProduction.CreateAt
                            })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            return result;
        }

        public List<EquipmentProductionItem> GetUseDKP(ModelFilter filter, int cateId, DateTime searchFromDate, DateTime searchToDate)
        {
            var dbo = ProductionDA.Repository;
            var lstEQDept = dbo.EquipmentProductionDepartments
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                 .Where(i => (i.StartTime <= searchToDate))
                                .Select(i => i.EquipmentProductionID)
                                .ToList();
            var result = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept.Contains(i.EquipmentProductionID) && i.IsFixed == true)
                                .Where(i => i.Time <= searchToDate && i.Time >= searchFromDate)
                            .Select(item => new EquipmentProductionItem()
                            {
                                ID = item.EquipmentProduction.ID,
                                Code = item.EquipmentProduction.Code,
                                Name = item.EquipmentProduction.Name,
                                SerialNumber = item.EquipmentProduction.SerialNumber,
                                UseStartTime = item.EquipmentProduction.UseStartTime,
                                MadeIn = item.EquipmentProduction.MadeIn,
                                MadeYear = item.EquipmentProduction.MadeYear,
                                IsActive = item.EquipmentProduction.IsActive,
                                IsDelete = item.EquipmentProduction.IsDelete,
                                IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                CreateAt = item.EquipmentProduction.CreateAt
                            })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            return result;
        }

        public List<EquipmentProductionItem> GetUsePass(ModelFilter filter, int cateId, DateTime searchFromDate, DateTime searchToDate, bool pass)
        {
            var dbo = ProductionDA.Repository;
            var lstEQDept = dbo.EquipmentProductionDepartments
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                 .Where(i => (i.StartTime <= searchToDate))
                                .Select(i => i.EquipmentProductionID)
                                .ToList();
            var lstMaintain = dbo.EquipmentProductionMaintains
                                  .Where(i => lstEQDept.Contains(i.EquipmentProductionID))
                                  .Where(i => i.PlanTime.HasValue && i.PlanTime >= searchFromDate && i.PlanTime <= searchToDate);
            var result = lstMaintain.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                                .Where(i => i.PlanTime.Value >= i.RealTime.Value)
                            .Select(item => new EquipmentProductionItem()
                            {
                                ID = item.EquipmentProduction.ID,
                                Code = item.EquipmentProduction.Code,
                                Name = item.EquipmentProduction.Name,
                                SerialNumber = item.EquipmentProduction.SerialNumber,
                                UseStartTime = item.EquipmentProduction.UseStartTime,
                                MadeIn = item.EquipmentProduction.MadeIn,
                                MadeYear = item.EquipmentProduction.MadeYear,
                                IsActive = item.EquipmentProduction.IsActive,
                                IsDelete = item.EquipmentProduction.IsDelete,
                                IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                CreateAt = item.EquipmentProduction.CreateAt
                            })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            if (!pass)
                result = lstMaintain.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                                .Where(i => i.PlanTime.Value < i.RealTime.Value)
                            .Select(item => new EquipmentProductionItem()
                            {
                                ID = item.EquipmentProduction.ID,
                                Code = item.EquipmentProduction.Code,
                                Name = item.EquipmentProduction.Name,
                                SerialNumber = item.EquipmentProduction.SerialNumber,
                                UseStartTime = item.EquipmentProduction.UseStartTime,
                                MadeIn = item.EquipmentProduction.MadeIn,
                                MadeYear = item.EquipmentProduction.MadeYear,
                                IsActive = item.EquipmentProduction.IsActive,
                                IsDelete = item.EquipmentProduction.IsDelete,
                                IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                CreateAt = item.EquipmentProduction.CreateAt
                            })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();

            return result;
        }

        public List<EquipmentProductionItem> GetTotalNotUse(ModelFilter filter, int cateId, DateTime searchFromDate, DateTime searchToDate)
        {
            var dbo = ProductionDA.Repository;
            var lstEQDept = dbo.EquipmentProductionDepartments
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                 .Where(i => (i.StartTime <= searchToDate))
                                .Select(i => i.EquipmentProductionID)
                                .ToList();
            var lstEQDept_not = dbo.EquipmentProductions
                                .Where(i => i.EquipmentProductionCategoryID == cateId)
                                .Where(i => !lstEQDept.Contains(i.ID) && i.UseStartTime <= searchToDate)
                                .Select(item => new EquipmentProductionItem()
                                {
                                    ID = item.ID,
                                    Code = item.Code,
                                    Name = item.Name,
                                    SerialNumber = item.SerialNumber,
                                    UseStartTime = item.UseStartTime,
                                    MadeIn = item.MadeIn,
                                    MadeYear = item.MadeYear,
                                    IsActive = item.IsActive,
                                    IsDelete = item.IsDelete,
                                    IsError = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                    IsFixed = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                    IsMaintain = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                    IsUsing = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                    CreateAt = item.CreateAt
                                })
                                .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            return lstEQDept_not;
        }

        public List<EquipmentProductionItem> GetNotUseErrors(ModelFilter filter, int cateId, DateTime searchFromDate, DateTime searchToDate, bool p)
        {
            var dbo = ProductionDA.Repository;
            var lstEQDept = dbo.EquipmentProductionDepartments
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                 .Where(i => (i.StartTime <= searchToDate))
                                .Select(i => i.EquipmentProductionID)
                                .ToList();
            var lstEQDept_not = dbo.EquipmentProductions
                                .Where(i => i.EquipmentProductionCategoryID == cateId)
                                .Where(i => !lstEQDept.Contains(i.ID) && i.UseStartTime <= searchToDate)
                                .Select(i => i.ID)
                                .ToList();
            var result = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                .Where(i => i.IsFixed == p)
                                .Where(i => i.Time <= searchToDate && i.Time >= searchFromDate)
                            .Select(item => new EquipmentProductionItem()
                            {
                                ID = item.EquipmentProduction.ID,
                                Code = item.EquipmentProduction.Code,
                                Name = item.EquipmentProduction.Name,
                                SerialNumber = item.EquipmentProduction.SerialNumber,
                                UseStartTime = item.EquipmentProduction.UseStartTime,
                                MadeIn = item.EquipmentProduction.MadeIn,
                                MadeYear = item.EquipmentProduction.MadeYear,
                                IsActive = item.EquipmentProduction.IsActive,
                                IsDelete = item.EquipmentProduction.IsDelete,
                                IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                CreateAt = item.EquipmentProduction.CreateAt
                            })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();

            return result;
        }


        public List<EquipmentProductionItem> GetAll()
        {
            var lstCategory = new List<EquipmentProductionCategoryItem>();
            lstCategory = GetCategory();

            var lstProduct = new List<EquipmentProductionItem>();
            var datenow = DateTime.Now;
            foreach (var index in lstCategory)
            {
                var SelectedCategory = ProductionDA.Repository.EquipmentProductionCategories.Where(i => i.ID == index.ID);
                var result = getChilds(SelectedCategory, index.ID)
                            .SelectMany(i => i.EquipmentProductions)
                            .Select(item => new EquipmentProductionItem()
                            {
                                ID = item.ID,
                                Code = item.Code,
                                Name = item.Name,
                                SerialNumber = item.SerialNumber,
                                UseStartTime = item.UseStartTime,
                                MadeIn = item.MadeIn,
                                MadeYear = item.MadeYear,
                                IsActive = item.IsActive,
                                IsDelete = item.IsDelete,
                                IsError = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                IsFixed = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                IsMaintain = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                IsUsing = item.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                CreateAt = item.CreateAt,
                                SupportInfomation = item.SupportInfomation
                            })
                            .OrderByDescending(item => item.CreateAt).ToList();
                foreach (var productItem in result)
                {
                    if (productItem.MadeYear != null)
                    {
                        var DateProduct = DateTime.Parse(productItem.MadeYear.ToString());
                        double result2 = DateProduct.Subtract(datenow).TotalDays;
                        if (result2 <= 0)
                        {
                            productItem.SerialNumber = "1";
                            lstProduct.Add(productItem);
                        }
                        else
                        {
                            if (result2 > 0 && result2 < 60)
                            {
                                productItem.SerialNumber = "2";
                                lstProduct.Add(productItem);
                            }
                            else
                            {
                                int id = productItem.ID;
                                var lstEquipmentProductionMaintainItem = EquipmentProductionMaintainDA.GetQuery(e => e.EquipmentProductionID == id)
                                .OrderByDescending(item => item.RealTime).ToList();

                                var DateMaintain = DateTime.Parse(lstEquipmentProductionMaintainItem[0].RealTime.ToString());
                                double result3 = DateMaintain.Subtract(datenow).TotalDays;
                                if (result3 <= 0)
                                {
                                    productItem.SerialNumber = "3";
                                    productItem.MadeYear = lstEquipmentProductionMaintainItem[0].RealTime;
                                    lstProduct.Add(productItem);
                                }
                                else
                                {
                                    if (result2 > 0 && result2 < 30)
                                    {
                                        productItem.SerialNumber = "4";
                                        productItem.MadeYear = lstEquipmentProductionMaintainItem[0].RealTime;
                                        lstProduct.Add(productItem);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            var lstCategoryMaintain = equipmentMeasureCategoryDA.GetQuery(e => !e.IsDelete);
            if (lstCategoryMaintain != null)
            {
                foreach (var index in lstCategory)
                {
                    var data = equipmentMeasureDA.GetQuery(t => t.EquipmentMeasureCategoryID == index.ID && !t.IsDelete)
                                        .Select(item => new EquipmentProductionItem
                                        {
                                            ID = item.ID,
                                            Code = item.Code,
                                            Name = item.Name,
                                            SerialNumber = item.SerialNumber,
                                            UseStartTime = item.UseStartTime,
                                            MadeIn = item.MadeIn,
                                            MadeYear = item.ExprireTime,
                                            IsActive = item.IsActive,
                                            IsDelete = item.IsDelete,
                                            Features = item.Features,
                                            IsError = item.IsError,
                                            IsFixed = item.IsFixed,
                                            IsUsing = item.IsUsing,
                                            Specifications = item.Specifications,
                                            SupportInfomation = item.SupportInfomation,
                                            UpdateAt = item.UpdateAt,
                                            UpdateBy = item.UpdateBy
                                        })
                                        .OrderByDescending(t => t.ID)
                                        .ToList();
                    foreach (var productItem in data)
                    {
                        if (productItem.MadeYear != null)
                        {
                            var DateProduct = DateTime.Parse(productItem.MadeYear.ToString());
                            double result2 = DateProduct.Subtract(datenow).TotalDays;
                            if (result2 <= 0)
                            {
                                productItem.SerialNumber = "1";
                                lstProduct.Add(productItem);
                            }
                            else
                            {
                                if (result2 > 0 && result2 < 60)
                                {
                                    productItem.SerialNumber = "2";
                                    lstProduct.Add(productItem);
                                }
                            }
                        }
                    }
                }
            }

            return lstProduct;
        }

        public List<EquipmentProductionItem> NotUsePass(ModelFilter filter, int cateId, DateTime searchFromDate, DateTime searchToDate, bool pass)
        {
            var dbo = ProductionDA.Repository;
            var lstEQDept = dbo.EquipmentProductionDepartments
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                 .Where(i => (i.StartTime <= searchToDate))
                                .Select(i => i.EquipmentProductionID)
                                .ToList();
            var lstEQDept_not = dbo.EquipmentProductions
                                .Where(i => i.EquipmentProductionCategoryID == cateId)
                                .Where(i => !lstEQDept.Contains(i.ID) && i.UseStartTime <= searchToDate)
                                .Select(i => i.ID)
                                .ToList();
            var lstMaintain_not = dbo.EquipmentProductionMaintains
                                  .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                  .Where(i => i.PlanTime.HasValue && i.PlanTime >= searchFromDate && i.PlanTime <= searchToDate);
            //Thiet bi bao duong dat
            //Thiet bi bao duong dat
            var rs = lstMaintain_not.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                            .Where(i => i.PlanTime.Value >= i.RealTime.Value)
                            .Select(item => new EquipmentProductionItem()
                            {
                                ID = item.EquipmentProduction.ID,
                                Code = item.EquipmentProduction.Code,
                                Name = item.EquipmentProduction.Name,
                                SerialNumber = item.EquipmentProduction.SerialNumber,
                                UseStartTime = item.EquipmentProduction.UseStartTime,
                                MadeIn = item.EquipmentProduction.MadeIn,
                                MadeYear = item.EquipmentProduction.MadeYear,
                                IsActive = item.EquipmentProduction.IsActive,
                                IsDelete = item.EquipmentProduction.IsDelete,
                                IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                CreateAt = item.EquipmentProduction.CreateAt
                            })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            if (!pass)
            {
                rs = lstMaintain_not.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                            .Where(i => i.PlanTime.Value < i.RealTime.Value)
                            .Select(item => new EquipmentProductionItem()
                            {
                                ID = item.EquipmentProduction.ID,
                                Code = item.EquipmentProduction.Code,
                                Name = item.EquipmentProduction.Name,
                                SerialNumber = item.EquipmentProduction.SerialNumber,
                                UseStartTime = item.EquipmentProduction.UseStartTime,
                                MadeIn = item.EquipmentProduction.MadeIn,
                                MadeYear = item.EquipmentProduction.MadeYear,
                                IsActive = item.EquipmentProduction.IsActive,
                                IsDelete = item.EquipmentProduction.IsDelete,
                                IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                CreateAt = item.EquipmentProduction.CreateAt
                            })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            }
            return rs;
        }
        public List<EquipmentProductionItem> GetByCategory_Department(ModelFilter filter, int cateId, int depId, DateTime fromDate, DateTime toDate)
        {
            var dbo = ProductionDA.Repository;
            var rs = dbo.EquipmentProductionDepartments
                                .Where(i => i.HumanDepartmentID == depId)
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                                .Where(i => (i.StartTime <= toDate))
                                .Select(item => new EquipmentProductionItem()
                                {
                                    ID = item.EquipmentProduction.ID,
                                    Code = item.EquipmentProduction.Code,
                                    Name = item.EquipmentProduction.Name,
                                    SerialNumber = item.EquipmentProduction.SerialNumber,
                                    UseStartTime = item.EquipmentProduction.UseStartTime,
                                    MadeIn = item.EquipmentProduction.MadeIn,
                                    MadeYear = item.EquipmentProduction.MadeYear,
                                    IsActive = item.EquipmentProduction.IsActive,
                                    IsDelete = item.EquipmentProduction.IsDelete,
                                    IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                    IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                    IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                    IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                    CreateAt = item.EquipmentProduction.CreateAt
                                })
                                .OrderByDescending(item => item.CreateAt)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return rs;
        }

        public List<EquipmentProductionItem> GetByCategory_DepartmentErrors(ModelFilter filter, int cateId, int depId, DateTime fromDate, DateTime toDate, bool pass)
        {
            var dbo = ProductionDA.Repository;
            var rs = dbo.EquipmentProductionErrors
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                                .Where(i => i.HumanDepartmentID == depId && i.IsFixed == pass)
                                .Where(i => (i.StartTime >= fromDate && i.StartTime <= toDate) || (i.EndTime >= fromDate && i.EndTime <= toDate) || (i.StartTime <= fromDate && i.EndTime >= toDate))
                                .Select(item => new EquipmentProductionItem()
                                {
                                    ID = item.EquipmentProduction.ID,
                                    Code = item.EquipmentProduction.Code,
                                    Name = item.EquipmentProduction.Name,
                                    SerialNumber = item.EquipmentProduction.SerialNumber,
                                    UseStartTime = item.EquipmentProduction.UseStartTime,
                                    MadeIn = item.EquipmentProduction.MadeIn,
                                    MadeYear = item.EquipmentProduction.MadeYear,
                                    IsActive = item.EquipmentProduction.IsActive,
                                    IsDelete = item.EquipmentProduction.IsDelete,
                                    IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                    IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                    IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                    IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                    CreateAt = item.EquipmentProduction.CreateAt
                                })
                                .OrderByDescending(item => item.CreateAt)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return rs;
        }
        public List<EquipmentProductionItem> GetByCategory_DepartmentMaintain(ModelFilter filter, int cateId, int depId, DateTime fromDate, DateTime toDate, bool pass)
        {
            var dbo = ProductionDA.Repository;
            var lstMaintain = dbo.EquipmentProductionMaintains
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == cateId)
                                 .Where(i => i.HumanDepartmentID == depId)
                                 .Where(i => i.PlanTime.HasValue && i.PlanTime >= fromDate && i.PlanTime <= toDate);
            var rs = lstMaintain.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                                .Where(i => i.PlanTime.Value >= i.RealTime.Value).Select(item => new EquipmentProductionItem()
                                {
                                    ID = item.EquipmentProduction.ID,
                                    Code = item.EquipmentProduction.Code,
                                    Name = item.EquipmentProduction.Name,
                                    SerialNumber = item.EquipmentProduction.SerialNumber,
                                    UseStartTime = item.EquipmentProduction.UseStartTime,
                                    MadeIn = item.EquipmentProduction.MadeIn,
                                    MadeYear = item.EquipmentProduction.MadeYear,
                                    IsActive = item.EquipmentProduction.IsActive,
                                    IsDelete = item.EquipmentProduction.IsDelete,
                                    IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                    IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                    IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                    IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                    CreateAt = item.EquipmentProduction.CreateAt
                                })
                                .OrderByDescending(item => item.CreateAt)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            if (!pass)
                rs = lstMaintain.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                                   .Where(i => i.PlanTime.Value < i.RealTime.Value)
                                   .Select(item => new EquipmentProductionItem()
                                   {
                                       ID = item.EquipmentProduction.ID,
                                       Code = item.EquipmentProduction.Code,
                                       Name = item.EquipmentProduction.Name,
                                       SerialNumber = item.EquipmentProduction.SerialNumber,
                                       UseStartTime = item.EquipmentProduction.UseStartTime,
                                       MadeIn = item.EquipmentProduction.MadeIn,
                                       MadeYear = item.EquipmentProduction.MadeYear,
                                       IsActive = item.EquipmentProduction.IsActive,
                                       IsDelete = item.EquipmentProduction.IsDelete,
                                       IsError = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsError).FirstOrDefault(),
                                       IsFixed = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsFixed).FirstOrDefault(),
                                       IsMaintain = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsMaintain).FirstOrDefault(),
                                       IsUsing = item.EquipmentProduction.EquipmentProductionHistories.OrderByDescending(i => i.Time).Select(i => i.IsUsing).FirstOrDefault(),
                                       CreateAt = item.EquipmentProduction.CreateAt
                                   })
                                   .OrderByDescending(item => item.CreateAt)
                                   .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                   .ToList();
            return rs;
        }
    }
}
