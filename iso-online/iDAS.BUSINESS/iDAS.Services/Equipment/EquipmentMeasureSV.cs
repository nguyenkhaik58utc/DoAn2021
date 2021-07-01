using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class EquipmentMeasureSV
    {
        private EquipmentMeasureDA equipmentMeasureDA = new EquipmentMeasureDA();
        private EquipmentMeasureCategoryDA equipmentMeasureCategoryDA = new EquipmentMeasureCategoryDA();
        public List<EquipmentMeasureItem> GetAll(int page, int pageSize, out int total, int cateId)
        {
            var dbo = equipmentMeasureDA.Repository;
            var data = equipmentMeasureDA.GetQuery(t => t.EquipmentMeasureCategoryID == cateId && !t.IsDelete)
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.ID,
                                    Code = item.Code,
                                    Accuracy = item.Accuracy,
                                    CalibrationLastTime = item.CalibrationLastTime,
                                    CalibrationNextTime = item.CalibrationLastTime,
                                    CalibrationPeriodic = item.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasureCategoryID,
                                    ExprireTime = item.ExprireTime,
                                    Features = item.Features,
                                    IsAccreditation = item.IsAccreditation,
                                    IsActive = item.IsActive,
                                    IsCalibration = item.IsCalibration,
                                    IsError = item.IsError,
                                    IsFixed = item.IsFixed,
                                    MadeIn = item.MadeIn,
                                    IsUsing = item.IsUsing,
                                    MadeYear = item.MadeYear,
                                    Name = item.Name,
                                    ScopeEnd = item.ScopeEnd,
                                    ScopeStart = item.ScopeStart,
                                    ScopeUnit = item.ScopeUnit,
                                    SerialNumber = item.SerialNumber,
                                    Specifications = item.Specifications,
                                    SupportInfomation = item.SupportInfomation,
                                    UseStartTime = item.UseStartTime,
                                    UpdateAt = item.UpdateAt,
                                    UpdateBy = item.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(page, pageSize, out total)
                                .ToList();
            return data;
        }
        public List<EquipmentProductionItem> GetList()
        {
            var dbo = equipmentMeasureDA.Repository;

            var lstCategory = equipmentMeasureCategoryDA.GetQuery(e => !e.IsDelete);
            if (lstCategory != null)
            {
                var lstProduct = new List<EquipmentProductionItem>();
                var datenow = DateTime.Now;
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
                    lstProduct.AddRange(data);
                }
                return lstProduct;
            }
            else {
                return null;
            }
        }
        public EquipmentMeasureItem GetById(int id)
        {
            var result = equipmentMeasureDA.GetQuery(i => i.ID == id)
                        .Select(item => new EquipmentMeasureItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Accuracy = item.Accuracy,
                            CalibrationLastTime = item.CalibrationLastTime,
                            EquipmentMeasureCategoryName = item.EquipmentMeasureCategory.Name,
                            CalibrationNextTime = item.CalibrationLastTime,
                            CalibrationPeriodic = item.CalibrationPeriodic,
                            EquipmentMeasureCategoryID = item.EquipmentMeasureCategoryID,
                            ExprireTime = item.ExprireTime,
                            Features = item.Features,
                            IsAccreditation = item.IsAccreditation,
                            IsActive = item.IsActive,
                            IsCalibration = item.IsCalibration,
                            IsError = item.IsError,
                            IsFixed = item.IsFixed,
                            MadeIn = item.MadeIn,
                            IsUsing = item.IsUsing,
                            MadeYear = item.MadeYear,
                            Name = item.Name,
                            ScopeEnd = item.ScopeEnd,
                            ScopeStart = item.ScopeStart,
                            ScopeUnit = item.ScopeUnit,
                            SerialNumber = item.SerialNumber,
                            Specifications = item.Specifications,
                            SupportInfomation = item.SupportInfomation,
                            UseStartTime = item.UseStartTime,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            /*if (result != null)
            {
                var files = equipmentMeasureDA.Repository.EquipmentMeasureAttachmentFiles.Where(i => i.EquipmentMeasureID == id)
                        .Select(i => i.AttachmentFileID).ToList();
                result.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = files
                };
            }*/
            return result;
        }
        /// <summary>
        /// Cập nhật  thiết bị đo lường
        /// || Author: CuongPC || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentMeasureItem item, int userID)
        {
            var measure = equipmentMeasureDA.GetById(item.ID);
            measure.Code = item.Code;
            measure.Accuracy = item.Accuracy;
            measure.CalibrationLastTime = item.CalibrationLastTime;
            measure.CalibrationNextTime = item.CalibrationLastTime;
            measure.CalibrationPeriodic = item.CalibrationPeriodic;
            measure.EquipmentMeasureCategoryID = item.EquipmentMeasureCategoryID;
            measure.ExprireTime = item.ExprireTime;
            measure.Features = item.Features;
            measure.IsAccreditation = item.IsAccreditation;
            measure.IsActive = item.IsActive;
            measure.IsCalibration = item.IsCalibration;
            measure.IsError = item.IsError;
            measure.IsFixed = item.IsFixed;
            measure.MadeIn = item.MadeIn;
            measure.IsUsing = item.IsUsing;
            measure.MadeYear = item.MadeYear;
            measure.Name = item.Name;
            measure.ScopeEnd = item.ScopeEnd;
            measure.ScopeStart = item.ScopeStart;
            measure.ScopeUnit = item.ScopeUnit;
            measure.SerialNumber = item.SerialNumber;
            measure.Specifications = item.Specifications;
            measure.SupportInfomation = item.SupportInfomation;
            measure.UseStartTime = item.UseStartTime;
            measure.UpdateAt = item.UpdateAt;
            measure.UpdateBy = item.UpdateBy;
            equipmentMeasureDA.Save();
            new FileSV().Upload<EquipmentMeasureAttachmentFile>(item.AttachmentFiles, item.ID);
        }
        /// <summary>
        /// Thêm mới  thiết bị đo lường
        /// || Author: CuongPC || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentMeasureItem item, int userID)
        {
            var equipmentMeasure = new EquipmentMeasure()
            {
                Code = item.Code,
                Accuracy = item.Accuracy,
                CalibrationLastTime = item.CalibrationLastTime,
                CalibrationNextTime = item.CalibrationLastTime,
                CalibrationPeriodic = item.CalibrationPeriodic,
                EquipmentMeasureCategoryID = item.EquipmentMeasureCategoryID,
                ExprireTime = item.ExprireTime,
                Features = item.Features,
                IsAccreditation = item.IsAccreditation,
                IsActive = item.IsActive,
                IsCalibration = item.IsCalibration,
                IsError = item.IsError,
                IsFixed = item.IsFixed,
                MadeIn = item.MadeIn,
                IsUsing = item.IsUsing,
                MadeYear = item.MadeYear,
                Name = item.Name,
                ScopeEnd = item.ScopeEnd,
                ScopeStart = item.ScopeStart,
                ScopeUnit = item.ScopeUnit,
                SerialNumber = item.SerialNumber,
                Specifications = item.Specifications,
                SupportInfomation = item.SupportInfomation,
                UseStartTime = item.UseStartTime,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            equipmentMeasureDA.Insert(equipmentMeasure);
            equipmentMeasureDA.Save();
            new FileSV().Upload<EquipmentMeasureAttachmentFile>(item.AttachmentFiles, equipmentMeasure.ID);
            return equipmentMeasure.ID;
        }
        /// <summary>
        /// Xóa  thiết bị đo lường
        /// || Author: CuongPC || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            equipmentMeasureDA.Delete(id);
            equipmentMeasureDA.Save();
        }

        public List<EquipmentMeasureItem> GetByCategory(ModelFilter filter, int cateId, DateTime searchFromDate, DateTime searchToDate)
        {
            var dbo = equipmentMeasureDA.Repository;
            var data = equipmentMeasureDA.GetQuery(t => t.EquipmentMeasureCategoryID == cateId && !t.IsDelete && t.UseStartTime <= searchToDate)
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.ID,
                                    Code = item.Code,
                                    Accuracy = item.Accuracy,
                                    CalibrationLastTime = item.CalibrationLastTime,
                                    CalibrationNextTime = item.CalibrationLastTime,
                                    CalibrationPeriodic = item.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasureCategoryID,
                                    ExprireTime = item.ExprireTime,
                                    Features = item.Features,
                                    IsAccreditation = item.IsAccreditation,
                                    IsActive = item.IsActive,
                                    IsCalibration = item.IsCalibration,
                                    IsError = item.IsError,
                                    IsFixed = item.IsFixed,
                                    MadeIn = item.MadeIn,
                                    IsUsing = item.IsUsing,
                                    MadeYear = item.MadeYear,
                                    Name = item.Name,
                                    ScopeEnd = item.ScopeEnd,
                                    ScopeStart = item.ScopeStart,
                                    ScopeUnit = item.ScopeUnit,
                                    SerialNumber = item.SerialNumber,
                                    Specifications = item.Specifications,
                                    SupportInfomation = item.SupportInfomation,
                                    UseStartTime = item.UseStartTime,
                                    UpdateAt = item.UpdateAt,
                                    UpdateBy = item.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return data;
        }
        public List<EquipmentMeasureItem> GetByCategoryUse(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = equipmentMeasureDA.Repository;
            var data = dbo.EquipmentMeasureDepartments
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                .Where(i => (i.StartTime <= toDate))
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.EquipmentMeasure.ID,
                                    Code = item.EquipmentMeasure.Code,
                                    Accuracy = item.EquipmentMeasure.Accuracy,
                                    CalibrationLastTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationNextTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationPeriodic = item.EquipmentMeasure.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasure.EquipmentMeasureCategoryID,
                                    ExprireTime = item.EquipmentMeasure.ExprireTime,
                                    Features = item.EquipmentMeasure.Features,
                                    IsAccreditation = item.EquipmentMeasure.IsAccreditation,
                                    IsActive = item.EquipmentMeasure.IsActive,
                                    IsCalibration = item.EquipmentMeasure.IsCalibration,
                                    IsError = item.EquipmentMeasure.IsError,
                                    IsFixed = item.EquipmentMeasure.IsFixed,
                                    MadeIn = item.EquipmentMeasure.MadeIn,
                                    IsUsing = item.EquipmentMeasure.IsUsing,
                                    MadeYear = item.EquipmentMeasure.MadeYear,
                                    Name = item.EquipmentMeasure.Name,
                                    ScopeEnd = item.EquipmentMeasure.ScopeEnd,
                                    ScopeStart = item.EquipmentMeasure.ScopeStart,
                                    ScopeUnit = item.EquipmentMeasure.ScopeUnit,
                                    SerialNumber = item.EquipmentMeasure.SerialNumber,
                                    Specifications = item.EquipmentMeasure.Specifications,
                                    SupportInfomation = item.EquipmentMeasure.SupportInfomation,
                                    UseStartTime = item.EquipmentMeasure.UseStartTime,
                                    UpdateAt = item.EquipmentMeasure.UpdateAt,
                                    UpdateBy = item.EquipmentMeasure.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return data;
        }

        public List<EquipmentMeasureItem> GetUseCKP(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate, bool isPass)
        {
            var dbo = equipmentMeasureDA.Repository;
            var lstEQDept = dbo.EquipmentMeasureDepartments
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                .Where(i => (i.StartTime <= toDate))
                                .Select(i => i.EquipmentMeasureID)
                                .ToList();
            var rs = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == isPass)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.EquipmentMeasure.ID,
                                    Code = item.EquipmentMeasure.Code,
                                    Accuracy = item.EquipmentMeasure.Accuracy,
                                    CalibrationLastTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationNextTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationPeriodic = item.EquipmentMeasure.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasure.EquipmentMeasureCategoryID,
                                    ExprireTime = item.EquipmentMeasure.ExprireTime,
                                    Features = item.EquipmentMeasure.Features,
                                    IsAccreditation = item.EquipmentMeasure.IsAccreditation,
                                    IsActive = item.EquipmentMeasure.IsActive,
                                    IsCalibration = item.EquipmentMeasure.IsCalibration,
                                    IsError = item.EquipmentMeasure.IsError,
                                    IsFixed = item.EquipmentMeasure.IsFixed,
                                    MadeIn = item.EquipmentMeasure.MadeIn,
                                    IsUsing = item.EquipmentMeasure.IsUsing,
                                    MadeYear = item.EquipmentMeasure.MadeYear,
                                    Name = item.EquipmentMeasure.Name,
                                    ScopeEnd = item.EquipmentMeasure.ScopeEnd,
                                    ScopeStart = item.EquipmentMeasure.ScopeStart,
                                    ScopeUnit = item.EquipmentMeasure.ScopeUnit,
                                    SerialNumber = item.EquipmentMeasure.SerialNumber,
                                    Specifications = item.EquipmentMeasure.Specifications,
                                    SupportInfomation = item.EquipmentMeasure.SupportInfomation,
                                    UseStartTime = item.EquipmentMeasure.UseStartTime,
                                    UpdateAt = item.EquipmentMeasure.UpdateAt,
                                    UpdateBy = item.EquipmentMeasure.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return rs;
        }

        public List<EquipmentMeasureItem> GetUsePass(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate, bool isPass)
        {
            var dbo = equipmentMeasureDA.Repository;
            var lstEQDept = dbo.EquipmentMeasureDepartments
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                .Where(i => (i.StartTime <= toDate))
                                .Select(i => i.EquipmentMeasureID)
                                .ToList();
            var rs = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == isPass)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.EquipmentMeasure.ID,
                                    Code = item.EquipmentMeasure.Code,
                                    Accuracy = item.EquipmentMeasure.Accuracy,
                                    CalibrationLastTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationNextTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationPeriodic = item.EquipmentMeasure.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasure.EquipmentMeasureCategoryID,
                                    ExprireTime = item.EquipmentMeasure.ExprireTime,
                                    Features = item.EquipmentMeasure.Features,
                                    IsAccreditation = item.EquipmentMeasure.IsAccreditation,
                                    IsActive = item.EquipmentMeasure.IsActive,
                                    IsCalibration = item.EquipmentMeasure.IsCalibration,
                                    IsError = item.EquipmentMeasure.IsError,
                                    IsFixed = item.EquipmentMeasure.IsFixed,
                                    MadeIn = item.EquipmentMeasure.MadeIn,
                                    IsUsing = item.EquipmentMeasure.IsUsing,
                                    MadeYear = item.EquipmentMeasure.MadeYear,
                                    Name = item.EquipmentMeasure.Name,
                                    ScopeEnd = item.EquipmentMeasure.ScopeEnd,
                                    ScopeStart = item.EquipmentMeasure.ScopeStart,
                                    ScopeUnit = item.EquipmentMeasure.ScopeUnit,
                                    SerialNumber = item.EquipmentMeasure.SerialNumber,
                                    Specifications = item.EquipmentMeasure.Specifications,
                                    SupportInfomation = item.EquipmentMeasure.SupportInfomation,
                                    UseStartTime = item.EquipmentMeasure.UseStartTime,
                                    UpdateAt = item.EquipmentMeasure.UpdateAt,
                                    UpdateBy = item.EquipmentMeasure.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return rs;
        }

        public List<EquipmentMeasureItem> GetNotUseErrors(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate, bool p)
        {
            var dbo = equipmentMeasureDA.Repository;
            var lstEQDept = dbo.EquipmentMeasureDepartments
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                .Where(i => (i.StartTime <= toDate))
                                .Select(i => i.EquipmentMeasureID)
                                .ToList();
            var lstEQDept_not = dbo.EquipmentMeasures
                                .Where(i => i.EquipmentMeasureCategoryID == cateId)
                                .Where(i => !lstEQDept.Contains(i.ID) && i.UseStartTime <= toDate)
                                .Select(i => i.ID)
                                .ToList();
            var rs = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == p)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.EquipmentMeasure.ID,
                                    Code = item.EquipmentMeasure.Code,
                                    Accuracy = item.EquipmentMeasure.Accuracy,
                                    CalibrationLastTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationNextTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationPeriodic = item.EquipmentMeasure.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasure.EquipmentMeasureCategoryID,
                                    ExprireTime = item.EquipmentMeasure.ExprireTime,
                                    Features = item.EquipmentMeasure.Features,
                                    IsAccreditation = item.EquipmentMeasure.IsAccreditation,
                                    IsActive = item.EquipmentMeasure.IsActive,
                                    IsCalibration = item.EquipmentMeasure.IsCalibration,
                                    IsError = item.EquipmentMeasure.IsError,
                                    IsFixed = item.EquipmentMeasure.IsFixed,
                                    MadeIn = item.EquipmentMeasure.MadeIn,
                                    IsUsing = item.EquipmentMeasure.IsUsing,
                                    MadeYear = item.EquipmentMeasure.MadeYear,
                                    Name = item.EquipmentMeasure.Name,
                                    ScopeEnd = item.EquipmentMeasure.ScopeEnd,
                                    ScopeStart = item.EquipmentMeasure.ScopeStart,
                                    ScopeUnit = item.EquipmentMeasure.ScopeUnit,
                                    SerialNumber = item.EquipmentMeasure.SerialNumber,
                                    Specifications = item.EquipmentMeasure.Specifications,
                                    SupportInfomation = item.EquipmentMeasure.SupportInfomation,
                                    UseStartTime = item.EquipmentMeasure.UseStartTime,
                                    UpdateAt = item.EquipmentMeasure.UpdateAt,
                                    UpdateBy = item.EquipmentMeasure.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return rs;
        }

        public List<EquipmentMeasureItem> GetTotalNotUse(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = equipmentMeasureDA.Repository;
            var lstEQDept = dbo.EquipmentMeasureDepartments
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                .Where(i => (i.StartTime <= toDate))
                                .Select(i => i.EquipmentMeasureID)
                                .ToList();
            var lstEQDept_not = dbo.EquipmentMeasures
                                .Where(i => i.EquipmentMeasureCategoryID == cateId)
                                .Where(i => !lstEQDept.Contains(i.ID) && i.UseStartTime <= toDate)
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.ID,
                                    Code = item.Code,
                                    Accuracy = item.Accuracy,
                                    CalibrationLastTime = item.CalibrationLastTime,
                                    CalibrationNextTime = item.CalibrationLastTime,
                                    CalibrationPeriodic = item.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasureCategoryID,
                                    ExprireTime = item.ExprireTime,
                                    Features = item.Features,
                                    IsAccreditation = item.IsAccreditation,
                                    IsActive = item.IsActive,
                                    IsCalibration = item.IsCalibration,
                                    IsError = item.IsError,
                                    IsFixed = item.IsFixed,
                                    MadeIn = item.MadeIn,
                                    IsUsing = item.IsUsing,
                                    MadeYear = item.MadeYear,
                                    Name = item.Name,
                                    ScopeEnd = item.ScopeEnd,
                                    ScopeStart = item.ScopeStart,
                                    ScopeUnit = item.ScopeUnit,
                                    SerialNumber = item.SerialNumber,
                                    Specifications = item.Specifications,
                                    SupportInfomation = item.SupportInfomation,
                                    UseStartTime = item.UseStartTime,
                                    UpdateAt = item.UpdateAt,
                                    UpdateBy = item.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return lstEQDept_not;
        }

        public List<EquipmentMeasureItem> NotUsePass(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate, bool p)
        {
            var dbo = equipmentMeasureDA.Repository;
            var lstEQDept = dbo.EquipmentMeasureDepartments
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                .Where(i => (i.StartTime <= toDate))
                                .Select(i => i.EquipmentMeasureID)
                                .ToList();
            var lstEQDept_not = dbo.EquipmentMeasures
                                .Where(i => i.EquipmentMeasureCategoryID == cateId)
                                .Where(i => !lstEQDept.Contains(i.ID) && i.UseStartTime <= toDate)
                                .Select(i => i.ID)
                                .ToList();
            var rs = dbo.EquipmentMeasureCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == p)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.EquipmentMeasure.ID,
                                    Code = item.EquipmentMeasure.Code,
                                    Accuracy = item.EquipmentMeasure.Accuracy,
                                    CalibrationLastTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationNextTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationPeriodic = item.EquipmentMeasure.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasure.EquipmentMeasureCategoryID,
                                    ExprireTime = item.EquipmentMeasure.ExprireTime,
                                    Features = item.EquipmentMeasure.Features,
                                    IsAccreditation = item.EquipmentMeasure.IsAccreditation,
                                    IsActive = item.EquipmentMeasure.IsActive,
                                    IsCalibration = item.EquipmentMeasure.IsCalibration,
                                    IsError = item.EquipmentMeasure.IsError,
                                    IsFixed = item.EquipmentMeasure.IsFixed,
                                    MadeIn = item.EquipmentMeasure.MadeIn,
                                    IsUsing = item.EquipmentMeasure.IsUsing,
                                    MadeYear = item.EquipmentMeasure.MadeYear,
                                    Name = item.EquipmentMeasure.Name,
                                    ScopeEnd = item.EquipmentMeasure.ScopeEnd,
                                    ScopeStart = item.EquipmentMeasure.ScopeStart,
                                    ScopeUnit = item.EquipmentMeasure.ScopeUnit,
                                    SerialNumber = item.EquipmentMeasure.SerialNumber,
                                    Specifications = item.EquipmentMeasure.Specifications,
                                    SupportInfomation = item.EquipmentMeasure.SupportInfomation,
                                    UseStartTime = item.EquipmentMeasure.UseStartTime,
                                    UpdateAt = item.EquipmentMeasure.UpdateAt,
                                    UpdateBy = item.EquipmentMeasure.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return rs;
        }

        public List<EquipmentMeasureItem> GetByCategory_Department(ModelFilter filter, int cateId, int depId, DateTime fromDate, DateTime toDate)
        {
            var dbo = equipmentMeasureDA.Repository;
            var lstEqDept = dbo.EquipmentMeasureDepartments
                                    .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                    .Where(i => i.HumanDepartmentID == depId)
                                    .Where(i => (i.StartTime <= toDate))
                                    .Distinct().Select(item => new EquipmentMeasureItem
                                    {
                                        ID = item.EquipmentMeasure.ID,
                                        Code = item.EquipmentMeasure.Code,
                                        Accuracy = item.EquipmentMeasure.Accuracy,
                                        CalibrationLastTime = item.EquipmentMeasure.CalibrationLastTime,
                                        CalibrationNextTime = item.EquipmentMeasure.CalibrationLastTime,
                                        CalibrationPeriodic = item.EquipmentMeasure.CalibrationPeriodic,
                                        EquipmentMeasureCategoryID = item.EquipmentMeasure.EquipmentMeasureCategoryID,
                                        ExprireTime = item.EquipmentMeasure.ExprireTime,
                                        Features = item.EquipmentMeasure.Features,
                                        IsAccreditation = item.EquipmentMeasure.IsAccreditation,
                                        IsActive = item.EquipmentMeasure.IsActive,
                                        IsCalibration = item.EquipmentMeasure.IsCalibration,
                                        IsError = item.EquipmentMeasure.IsError,
                                        IsFixed = item.EquipmentMeasure.IsFixed,
                                        MadeIn = item.EquipmentMeasure.MadeIn,
                                        IsUsing = item.EquipmentMeasure.IsUsing,
                                        MadeYear = item.EquipmentMeasure.MadeYear,
                                        Name = item.EquipmentMeasure.Name,
                                        ScopeEnd = item.EquipmentMeasure.ScopeEnd,
                                        ScopeStart = item.EquipmentMeasure.ScopeStart,
                                        ScopeUnit = item.EquipmentMeasure.ScopeUnit,
                                        SerialNumber = item.EquipmentMeasure.SerialNumber,
                                        Specifications = item.EquipmentMeasure.Specifications,
                                        SupportInfomation = item.EquipmentMeasure.SupportInfomation,
                                        UseStartTime = item.EquipmentMeasure.UseStartTime,
                                        UpdateAt = item.EquipmentMeasure.UpdateAt,
                                        UpdateBy = item.EquipmentMeasure.UpdateBy
                                    })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return lstEqDept;
        }

        public List<EquipmentMeasureItem> GetByCategory_DepartmentUsing(ModelFilter filter, int cateId, int depId, DateTime fromDate, DateTime toDate)
        {
            var dbo = equipmentMeasureDA.Repository;
            var lstEqDept = dbo.EquipmentMeasureDepartments
                                    .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                    .Where(i => i.HumanDepartmentID == depId && i.IsUsing == true)
                                    .Where(i => (i.StartTime <= toDate))
                                    .Distinct().Select(item => new EquipmentMeasureItem
                                    {
                                        ID = item.EquipmentMeasure.ID,
                                        Code = item.EquipmentMeasure.Code,
                                        Accuracy = item.EquipmentMeasure.Accuracy,
                                        CalibrationLastTime = item.EquipmentMeasure.CalibrationLastTime,
                                        CalibrationNextTime = item.EquipmentMeasure.CalibrationLastTime,
                                        CalibrationPeriodic = item.EquipmentMeasure.CalibrationPeriodic,
                                        EquipmentMeasureCategoryID = item.EquipmentMeasure.EquipmentMeasureCategoryID,
                                        ExprireTime = item.EquipmentMeasure.ExprireTime,
                                        Features = item.EquipmentMeasure.Features,
                                        IsAccreditation = item.EquipmentMeasure.IsAccreditation,
                                        IsActive = item.EquipmentMeasure.IsActive,
                                        IsCalibration = item.EquipmentMeasure.IsCalibration,
                                        IsError = item.EquipmentMeasure.IsError,
                                        IsFixed = item.EquipmentMeasure.IsFixed,
                                        MadeIn = item.EquipmentMeasure.MadeIn,
                                        IsUsing = item.EquipmentMeasure.IsUsing,
                                        MadeYear = item.EquipmentMeasure.MadeYear,
                                        Name = item.EquipmentMeasure.Name,
                                        ScopeEnd = item.EquipmentMeasure.ScopeEnd,
                                        ScopeStart = item.EquipmentMeasure.ScopeStart,
                                        ScopeUnit = item.EquipmentMeasure.ScopeUnit,
                                        SerialNumber = item.EquipmentMeasure.SerialNumber,
                                        Specifications = item.EquipmentMeasure.Specifications,
                                        SupportInfomation = item.EquipmentMeasure.SupportInfomation,
                                        UseStartTime = item.EquipmentMeasure.UseStartTime,
                                        UpdateAt = item.EquipmentMeasure.UpdateAt,
                                        UpdateBy = item.EquipmentMeasure.UpdateBy
                                    })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return lstEqDept;
        }

        public List<EquipmentMeasureItem> GetByCategory_DepartmentErrors(ModelFilter filter, int cateId, int depId, DateTime fromDate, DateTime toDate, bool p)
        {
            var dbo = equipmentMeasureDA.Repository;
            var lsEquIdForDeptUse = dbo.EquipmentMeasureDepartments
                                    .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                    .Where(i => i.HumanDepartmentID == depId && i.IsUsing == true)
                                    .Where(i => (i.StartTime <= toDate))
                                    .Select(i => i.EquipmentMeasureID);
            var rs = dbo.EquipmentCalibrations
                                .Where(i => lsEquIdForDeptUse.Contains(i.EquipmentMeasureID))
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                .Where(i => i.IsPass == p)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.EquipmentMeasure.ID,
                                    Code = item.EquipmentMeasure.Code,
                                    Accuracy = item.EquipmentMeasure.Accuracy,
                                    CalibrationLastTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationNextTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationPeriodic = item.EquipmentMeasure.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasure.EquipmentMeasureCategoryID,
                                    ExprireTime = item.EquipmentMeasure.ExprireTime,
                                    Features = item.EquipmentMeasure.Features,
                                    IsAccreditation = item.EquipmentMeasure.IsAccreditation,
                                    IsActive = item.EquipmentMeasure.IsActive,
                                    IsCalibration = item.EquipmentMeasure.IsCalibration,
                                    IsError = item.EquipmentMeasure.IsError,
                                    IsFixed = item.EquipmentMeasure.IsFixed,
                                    MadeIn = item.EquipmentMeasure.MadeIn,
                                    IsUsing = item.EquipmentMeasure.IsUsing,
                                    MadeYear = item.EquipmentMeasure.MadeYear,
                                    Name = item.EquipmentMeasure.Name,
                                    ScopeEnd = item.EquipmentMeasure.ScopeEnd,
                                    ScopeStart = item.EquipmentMeasure.ScopeStart,
                                    ScopeUnit = item.EquipmentMeasure.ScopeUnit,
                                    SerialNumber = item.EquipmentMeasure.SerialNumber,
                                    Specifications = item.EquipmentMeasure.Specifications,
                                    SupportInfomation = item.EquipmentMeasure.SupportInfomation,
                                    UseStartTime = item.EquipmentMeasure.UseStartTime,
                                    UpdateAt = item.EquipmentMeasure.UpdateAt,
                                    UpdateBy = item.EquipmentMeasure.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return rs;
        }

        public List<EquipmentMeasureItem> GetByCategory_DepartmentMaintain(ModelFilter filter, int cateId, int depId, DateTime fromDate, DateTime toDate, bool p)
        {
            var dbo = equipmentMeasureDA.Repository;
            var lsEquIdForDeptUse = dbo.EquipmentMeasureDepartments
                                    .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                    .Where(i => i.HumanDepartmentID == depId && i.IsUsing == true)
                                    .Where(i => (i.StartTime <= toDate))
                                    .Select(i => i.EquipmentMeasureID);
            var rs = dbo.EquipmentMeasureCalibrations
                                .Where(i => lsEquIdForDeptUse.Contains(i.EquipmentMeasureID))
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == cateId)
                                .Where(i => i.IsPass == p)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Select(item => new EquipmentMeasureItem
                                {
                                    ID = item.EquipmentMeasure.ID,
                                    Code = item.EquipmentMeasure.Code,
                                    Accuracy = item.EquipmentMeasure.Accuracy,
                                    CalibrationLastTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationNextTime = item.EquipmentMeasure.CalibrationLastTime,
                                    CalibrationPeriodic = item.EquipmentMeasure.CalibrationPeriodic,
                                    EquipmentMeasureCategoryID = item.EquipmentMeasure.EquipmentMeasureCategoryID,
                                    ExprireTime = item.EquipmentMeasure.ExprireTime,
                                    Features = item.EquipmentMeasure.Features,
                                    IsAccreditation = item.EquipmentMeasure.IsAccreditation,
                                    IsActive = item.EquipmentMeasure.IsActive,
                                    IsCalibration = item.EquipmentMeasure.IsCalibration,
                                    IsError = item.EquipmentMeasure.IsError,
                                    IsFixed = item.EquipmentMeasure.IsFixed,
                                    MadeIn = item.EquipmentMeasure.MadeIn,
                                    IsUsing = item.EquipmentMeasure.IsUsing,
                                    MadeYear = item.EquipmentMeasure.MadeYear,
                                    Name = item.EquipmentMeasure.Name,
                                    ScopeEnd = item.EquipmentMeasure.ScopeEnd,
                                    ScopeStart = item.EquipmentMeasure.ScopeStart,
                                    ScopeUnit = item.EquipmentMeasure.ScopeUnit,
                                    SerialNumber = item.EquipmentMeasure.SerialNumber,
                                    Specifications = item.EquipmentMeasure.Specifications,
                                    SupportInfomation = item.EquipmentMeasure.SupportInfomation,
                                    UseStartTime = item.EquipmentMeasure.UseStartTime,
                                    UpdateAt = item.EquipmentMeasure.UpdateAt,
                                    UpdateBy = item.EquipmentMeasure.UpdateBy
                                })
                                .OrderByDescending(t => t.ID)
                                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                                .ToList();
            return rs;
        }

        public bool checkCodeExist(string Code, int id = 0)
        {
            var lst = equipmentMeasureDA.GetQuery(x => x.Code == Code);
            if(id != 0 &&
                Code == equipmentMeasureDA.GetQuery(x => x.ID == id)
                            .Select(i=>i.Code).FirstOrDefault()
                )
            {
                return false;
            }
            if (lst.Count() > 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
