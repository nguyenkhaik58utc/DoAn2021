using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class EquipmentStatisticalModel
    {
        public string Name
        {
            get;
            set;
        }
        public int CateID { get; set; }
        public int? ParentID { get; set; }
        public bool Leaf { get; set; }
        public double Data1
        {
            get;
            set;
        }
        public double Data2
        {
            get;
            set;
        }
        public double Data3
        {
            get;
            set;
        }
        public double Data4
        {
            get;
            set;
        }
        public double Data5
        {
            get;
            set;
        }
        public double Data6
        {
            get;
            set;
        }
        public double Data7
        {
            get;
            set;
        }
        public double Data8
        {
            get;
            set;
        }
        public double Data9
        {
            get;
            set;
        }
        public double Data21
        {
            get;
            set;
        }
        public double Data31
        {
            get;
            set;
        }
        public double Data41
        {
            get;
            set;
        }
        public double Data51
        {
            get;
            set;
        }
        public double Data61
        {
            get;
            set;
        }
        public double Data71
        {
            get;
            set;
        }
    }
    public class EquipmentStatisticalSV
    {
        EquipmentProductionDepartmentDA eqProductDeptDA = new EquipmentProductionDepartmentDA();
        public List<EquipmentStatisticalModel> GenerateDataEquipmentProductionDepartmentStatistical(int DepID, int id, DateTime fromDate, DateTime toDate)
        {
            var dbo = eqProductDeptDA.Repository;
            var results = dbo.EquipmentProductionCategories
                            .Where(i => (i.ParentID != null && i.ParentID == id))
                             .Select(item => new EquipmentStatisticalModel()
                             {
                                 CateID = item.ID,
                                 ParentID = item.ParentID,
                                 Name = item.Name,
                                 Leaf = !dbo.EquipmentProductionCategories.Any(i => i.ParentID == item.ID)
                             }).ToList();
            foreach (var item in results)
            {
                //var lstChild = new EquipmentProductionCategorySV().getChilds(dbo.EquipmentProductionCategories, item.CateID).Select(i => i.ID).ToList();
                //if (!lstChild.Contains(item.CateID)) lstChild.Add(item.CateID);
                //Tong thiet bi duoc phan phoi
                item.Data1 = dbo.EquipmentProductionDepartments
                                .Where(i => i.HumanDepartmentID == DepID)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == item.CateID)
                                .Where(i => (i.StartTime <= toDate))
                                .Count();
                //Thiet bi su dung
                item.Data2 = dbo.EquipmentProductionDepartments
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == item.CateID)
                                .Where(i => i.HumanDepartmentID == DepID && i.IsUsing == true)
                                .Where(i => (i.StartTime <= toDate))
                                .Count();
                //Thiet bi su co chua xu ly
                item.Data3 = dbo.EquipmentProductionErrors
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == item.CateID)
                                .Where(i => i.HumanDepartmentID == DepID && i.IsFixed == false && i.IsNew == true)
                                .Where(i => (i.StartTime >= fromDate && i.StartTime <= toDate) || (i.EndTime >= fromDate && i.EndTime <= toDate) || (i.StartTime <= fromDate && i.EndTime >= toDate))
                                .Count();
                //Thiet bi su co dang xu ly
                item.Data4 = dbo.EquipmentProductionErrors
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == item.CateID)
                                .Where(i => i.HumanDepartmentID == DepID && i.IsFixed == false && i.IsNew == false)
                                .Where(i => (i.StartTime >= fromDate && i.StartTime <= toDate) || (i.EndTime >= fromDate && i.EndTime <= toDate) || (i.StartTime <= fromDate && i.EndTime >= toDate))
                                .Count();
                //Thiet bi su co da xu ly
                item.Data5 = dbo.EquipmentProductionErrors
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == item.CateID)
                                .Where(i => i.HumanDepartmentID == DepID && i.IsFixed == true)
                                .Where(i => (i.StartTime >= fromDate && i.StartTime <= toDate) || (i.EndTime >= fromDate && i.EndTime <= toDate) || (i.StartTime <= fromDate && i.EndTime >= toDate))
                                .Count();
                //Thiet bi bao duong dat
                var lstMaintain = dbo.EquipmentProductionMaintains
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == item.CateID)
                                 .Where(i => i.HumanDepartmentID == DepID)
                                 .Where(i => i.PlanTime.HasValue && i.PlanTime >= fromDate && i.PlanTime <= toDate);
                item.Data6 = lstMaintain.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                                .Where(i => i.PlanTime.Value >= i.RealTime.Value)
                                .Count();

                //Tbi bao duong khong dat
                item.Data7 = lstMaintain.Count() - item.Data6;

            }
            return results;
        }

        public List<EquipmentStatisticalModel> GenerateDataEquipmentProductionCategoryStatistical(int id, DateTime fromDate, DateTime toDate)
        {
            var dbo = eqProductDeptDA.Repository;
            var results = dbo.EquipmentProductionCategories
                            .Where(i => (i.ParentID != null && i.ParentID == id))
                             .Select(item => new EquipmentStatisticalModel()
                             {
                                 CateID = item.ID,
                                 ParentID = item.ParentID,
                                 Name = item.Name,
                                 Leaf = !dbo.EquipmentProductionCategories.Any(i => i.ParentID == item.ID)
                             }).ToList();

            foreach (var item in results)
            {
                //var lstChild = new EquipmentProductionCategorySV().getChilds(dbo.EquipmentProductionCategories, item.CateID).Select(i => i.ID).ToList();
                //if (!lstChild.Contains(item.CateID)) lstChild.Add(item.CateID);
                //Tong so thiet bi
                item.Data1 = dbo.EquipmentProductions
                                .Where(i => i.EquipmentProductionCategoryID == item.CateID)
                                .Where(i => i.UseStartTime <= toDate)
                                .Count();

                ///////////////////////////////////////////////////////////////////////////////////////
                //Thiet bi phan phoi
                //Tong so
                var lstEQDept = dbo.EquipmentProductionDepartments
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID == item.CateID)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                 .Where(i => (i.StartTime<= toDate))
                                .Select(i => i.EquipmentProductionID)
                                .ToList();
                item.Data2 = lstEQDept.Count;
                //Thiet bi su co chua xu ly
                item.Data3 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept.Contains(i.EquipmentProductionID) && i.IsFixed == false && i.IsNew == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                //Thiet bi su co dang xu ly
                item.Data4 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept.Contains(i.EquipmentProductionID) && i.IsFixed == false && i.IsNew == false)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                //Thiet bi su co da xu ly
                item.Data5 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept.Contains(i.EquipmentProductionID) && i.IsFixed == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                ///////
                var lstMaintain = dbo.EquipmentProductionMaintains
                                  .Where(i => lstEQDept.Contains(i.EquipmentProductionID))
                                  .Where(i => i.PlanTime.HasValue && i.PlanTime >= fromDate && i.PlanTime <= toDate);

                //Thiet bi bao duong dat
                item.Data6 = lstMaintain.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                                .Where(i => i.PlanTime.Value >= i.RealTime.Value && fromDate <= i.PlanTime.Value && i.PlanTime.Value <= toDate)
                                .Count();

                //Tbi bao duong khong dat
                item.Data7 = lstMaintain.Count() - item.Data6;

                ////////////////////////////////////////////////////////////////////////////////////////////
                //Thiet bi chua phan phoi
                //Tong so
                var lstEQDept_not = dbo.EquipmentProductions
                                .Where(i => i.EquipmentProductionCategoryID == item.CateID)
                                .Where(i => !lstEQDept.Contains(i.ID) && i.UseStartTime <= toDate)
                                .Select(i => i.ID)
                                .ToList();
                item.Data21 = lstEQDept_not.Count;
                //Thiet bi su co chua xu ly
                item.Data31 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                .Where(i => i.IsFixed == false && i.IsNew == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                //Thiet bi su co dang xu ly
                item.Data41 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                .Where(i => i.IsFixed == false && i.IsNew == false)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                //Thiet bi su co da xu ly
                item.Data51 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                .Where(i => i.IsFixed == true)//lstChild.Contains(i.EquipmentProduction.EquipmentProductionCategoryID) &&
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                ////
                var lstMaintain_not = dbo.EquipmentProductionMaintains
                                  .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                  .Where(i => i.PlanTime.HasValue && i.PlanTime >= fromDate && i.PlanTime <= toDate);
                //Thiet bi bao duong dat
                //Thiet bi bao duong dat
                item.Data61 = lstMaintain_not.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                                .Where(i => i.PlanTime.Value >= i.RealTime.Value && fromDate <= i.PlanTime.Value && i.PlanTime.Value <= toDate)
                                .Count();

                //Tbi bao duong khong dat
                item.Data71 = lstMaintain_not.Count() - item.Data61;

            }
            return results;
        }

        public List<EquipmentStatisticalModel> GenerateDataEquipmentProductionCategoryStatisticalByID(int id, DateTime fromDate, DateTime toDate)
        {
            var dbo = eqProductDeptDA.Repository;
            var results = dbo.EquipmentProductionCategories
                            .Where(i => i.ID == id)
                             .Select(item => new EquipmentStatisticalModel()
                             {
                                 CateID = item.ID,
                                 ParentID = item.ParentID,
                                 Name = item.Name,
                                 Leaf = !dbo.EquipmentProductionCategories.Any(i => i.ParentID == item.ID)
                             }).ToList();

            foreach (var item in results)
            {
                //var lstChild = new EquipmentProductionCategorySV().getChilds(dbo.EquipmentProductionCategories, item.CateID).Select(i => i.ID).ToList();
                //if (!lstChild.Contains(item.CateID)) lstChild.Add(item.CateID);
                //Tong so thiet bi
                item.Data1 = dbo.EquipmentProductions
                                .Where(i => i.EquipmentProductionCategoryID==item.CateID && i.UseStartTime <= toDate)
                                .Count();

                ///////////////////////////////////////////////////////////////////////////////////////
                //Thiet bi phan phoi
                //Tong so
                var lstEQDept = dbo.EquipmentProductionDepartments
                                .Where(i => i.EquipmentProduction.EquipmentProductionCategoryID== item.CateID && i.IsUsing == true)//Thu hồi using = false
                                .Where(i => (i.StartTime <= toDate))
                                .Select(i => i.EquipmentProductionID)
                                .ToList();
                item.Data2 = lstEQDept.Count;
                //Thiet bi su co chua xu ly
                item.Data3 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept.Contains(i.EquipmentProductionID) && i.IsFixed == false && i.IsNew == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                //Thiet bi su co dang xu ly
                item.Data4 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept.Contains(i.EquipmentProductionID) && i.IsFixed == false && i.IsNew == false)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                //Thiet bi su co da xu ly
                item.Data5 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept.Contains(i.EquipmentProductionID) && i.IsFixed == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                ///////
                var lstMaintain = dbo.EquipmentProductionMaintains
                                  .Where(i => lstEQDept.Contains(i.EquipmentProductionID))
                                  .Where(i => i.PlanTime.HasValue && i.PlanTime >= fromDate && i.PlanTime <= toDate);

                //Thiet bi bao duong dat
                item.Data6 = lstMaintain.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                                .Where(i => i.PlanTime.Value >= i.RealTime.Value && fromDate <= i.PlanTime.Value && i.PlanTime.Value <= toDate)
                                .Count();

                //Tbi bao duong khong dat
                item.Data7 = lstMaintain.Count() - item.Data6;

                ////////////////////////////////////////////////////////////////////////////////////////////
                //Thiet bi chua phan phoi
                //Tong so
                var lstEQDept_not = dbo.EquipmentProductions
                                .Where(i => !lstEQDept.Contains(i.ID) && i.EquipmentProductionCategoryID==item.CateID && i.UseStartTime <= toDate)
                                .Select(i => i.ID)
                                .ToList();
                item.Data21 = lstEQDept_not.Count;
                //Thiet bi su co chua xu ly
                item.Data31 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                .Where(i => i.IsFixed == false && i.IsNew == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                //Thiet bi su co dang xu ly
                item.Data41 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                .Where(i => i.IsFixed == false && i.IsNew == false)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                //Thiet bi su co da xu ly
                item.Data51 = dbo.EquipmentProductionErrors
                                .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                .Where(i => i.IsFixed == true)//lstChild.Contains(i.EquipmentProduction.EquipmentProductionCategoryID) &&
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();
                ////
                var lstMaintain_not = dbo.EquipmentProductionMaintains
                                  .Where(i => lstEQDept_not.Contains(i.EquipmentProductionID))
                                  .Where(i => i.PlanTime.HasValue && i.PlanTime >= fromDate && i.PlanTime <= toDate);
                //Thiet bi bao duong dat
                //Thiet bi bao duong dat
                item.Data61 = lstMaintain_not.Where(i => i.PlanTime.HasValue && i.RealTime.HasValue)
                                .Where(i => i.PlanTime.Value >= i.RealTime.Value && fromDate <= i.PlanTime.Value && i.PlanTime.Value <= toDate)
                                .Count();

                //Tbi bao duong khong dat
                item.Data71 = lstMaintain_not.Count() - item.Data61;

            }
            return results;
        }

        public List<EquipmentStatisticalModel> GenerateDataEquipmentMeasuresCategoryStatisticalByID(int id, DateTime fromDate, DateTime toDate)
        {
            var dbo = eqProductDeptDA.Repository;
            var results = dbo.EquipmentMeasureCategories
                            .Where(i => (i.ID == id))
                             .Select(item => new EquipmentStatisticalModel()
                             {
                                 CateID = item.ID,
                                 ParentID = item.ParentID,
                                 Name = item.Name,
                                 Leaf = !dbo.EquipmentMeasureCategories.Any(i => i.ParentID == item.ID)
                             }).ToList();

            foreach (var item in results)
            {
                //var lstChild = new EquipmentMeasureCategorySV().getChilds(dbo.EquipmentMeasureCategories, item.CateID).Select(i => i.ID).ToList();
                //if (!lstChild.Contains(item.CateID)) lstChild.Add(item.CateID);
                //Tong so thiet bi
                item.Data1 = dbo.EquipmentMeasures
                                .Where(i => i.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => i.UseStartTime <= toDate)
                                .Count();

                ///////////////////////////////////////////////////////////////////////////////////////
                //Thiet bi cấp phát
                //Tong so
                var lstEQDept = dbo.EquipmentMeasureDepartments
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                .Where(i => (i.StartTime <= toDate))
                                .Select(i => i.EquipmentMeasureID)
                                .ToList();
                item.Data2 = lstEQDept.Count;
                //Thiet bi Kiem dinh dat
                item.Data3 = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == true)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();
                //Thiet bi kiem dinh khong dat
                item.Data4 = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == false)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();
                //Thiet bi hieu chuan dat
                item.Data5 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

                //Tbi hieu chuan khong dat
                item.Data6 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == false)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

                ////////////////////////////////////////////////////////////////////////////////////////////
                //Thiet bi chua cap phat
                //Tong so
                var lstEQDept_not = dbo.EquipmentMeasures
                                .Where(i => i.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => !lstEQDept.Contains(i.ID) && i.UseStartTime <= toDate)
                                .Select(i => i.ID)
                                .ToList();
                item.Data21 = lstEQDept_not.Count;
                //Thiet bi kiem dinh dat
                item.Data31 = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == true)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();
                //Thiet bi kiem dinh khong dat
                item.Data41 = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == false)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();

                //Thiet bi hieu chuan dat
                item.Data51 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

                //Tbi hieu chuan khong dat
                item.Data61 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == false)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

            }
            return results;
        }
        /// <summary>
        /// Thống kê thiết bị đo theo nhóm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<EquipmentStatisticalModel> GenerateDataEquipmentCategoryStatistical(int id, DateTime fromDate, DateTime toDate)
        {
            var dbo = eqProductDeptDA.Repository;
            var results = dbo.EquipmentMeasureCategories
                            .Where(i => (i.ParentID != null && i.ParentID == id))
                             .Select(item => new EquipmentStatisticalModel()
                             {
                                 CateID = item.ID,
                                 ParentID = item.ParentID,
                                 Name = item.Name,
                                 Leaf = !dbo.EquipmentMeasureCategories.Any(i => i.ParentID == item.ID)
                             }).ToList();

            foreach (var item in results)
            {
                //var lstChild = new EquipmentMeasureCategorySV().getChilds(dbo.EquipmentMeasureCategories, item.CateID).Select(i => i.ID).ToList();
                //if (!lstChild.Contains(item.CateID)) lstChild.Add(item.CateID);
                //Tong so thiet bi
                item.Data1 = dbo.EquipmentMeasures
                                .Where(i => i.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => i.UseStartTime <= toDate)
                                .Count();

                ///////////////////////////////////////////////////////////////////////////////////////
                //Thiet bi cấp phát
                //Tong so
                var lstEQDept = dbo.EquipmentMeasureDepartments
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => i.IsUsing == true)//Thu hồi using = false
                                .Where(i => (i.StartTime <= toDate))
                                .Select(i => i.EquipmentMeasureID)
                                .ToList();
                item.Data2 = lstEQDept.Count;
                //Thiet bi Kiem dinh dat
                item.Data3 = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == true)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();
                //Thiet bi kiem dinh khong dat
                item.Data4 = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == false)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();
                //Thiet bi hieu chuan dat
                item.Data5 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

                //Tbi hieu chuan khong dat
                item.Data6 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lstEQDept.Contains(i.EquipmentMeasureID) && i.IsPass == false)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

                ////////////////////////////////////////////////////////////////////////////////////////////
                //Thiet bi chua cap phat
                //Tong so
                var lstEQDept_not = dbo.EquipmentMeasures
                                .Where(i => i.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => !lstEQDept.Contains(i.ID) && i.UseStartTime <= toDate)
                                .Select(i => i.ID)
                                .ToList();
                item.Data21 = lstEQDept_not.Count;
                //Thiet bi kiem dinh dat
                item.Data31 = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == true)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();
                //Thiet bi kiem dinh khong dat
                item.Data41 = dbo.EquipmentCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == false)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();

                //Thiet bi hieu chuan dat
                item.Data51 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

                //Tbi hieu chuan khong dat
                item.Data61 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lstEQDept_not.Contains(i.EquipmentMeasureID) && i.IsPass == false)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

            }
            return results;
        }
        /// <summary>
        /// Lấy danh sách thiết bị theo phòng ban
        /// </summary>
        /// <param name="DepID"></param>
        /// <param name="id"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<EquipmentStatisticalModel> GenerateDataEquipmentPDepartmentStatistical(int DepID, int id, DateTime fromDate, DateTime toDate)
        {
            var dbo = eqProductDeptDA.Repository;
            var results = dbo.EquipmentMeasureCategories
                            .Where(i => (i.ParentID != null && i.ParentID == id))
                             .Select(item => new EquipmentStatisticalModel()
                             {
                                 CateID = item.ID,
                                 ParentID = item.ParentID,
                                 Name = item.Name,
                                 Leaf = !dbo.EquipmentMeasureCategories.Any(i => i.ParentID == item.ID)
                             }).ToList();
            foreach (var item in results)
            {
                //Tong thiet bi
                var lstEqDept = dbo.EquipmentMeasureDepartments
                                    .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == item.CateID)
                                    .Where(i => i.HumanDepartmentID == DepID)
                                    .Where(i => (i.StartTime <= toDate))
                                    .Distinct();
                item.Data1 = lstEqDept.Count();
                //Thiet bi su dung
                var lsEquIdForDeptUse = lstEqDept.Where(i => i.IsUsing == true).Select(i=>i.EquipmentMeasureID);
                item.Data2 = lsEquIdForDeptUse.Count();
                //Thiet bi Kiem dinh dat
                item.Data3 = dbo.EquipmentCalibrations
                                .Where(i => lsEquIdForDeptUse.Contains(i.EquipmentMeasureID))
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => i.IsPass == true)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();
                //Thiet bi kiem dinh khong dat
                item.Data4 = dbo.EquipmentCalibrations
                                .Where(i => lsEquIdForDeptUse.Contains(i.EquipmentMeasureID))
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => i.IsPass == false)
                                .Where(i => i.AccreditationLastTime <= toDate && i.AccreditationLastTime >= fromDate)
                                .Count();

                //Thiet bi hieu chuan dat
                item.Data5 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lsEquIdForDeptUse.Contains(i.EquipmentMeasureID))
                                .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => i.IsPass == true)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

                //Tbi hieu chuan khong dat
                item.Data6 = dbo.EquipmentMeasureCalibrations
                                .Where(i => lsEquIdForDeptUse.Contains(i.EquipmentMeasureID))
                                 .Where(i => i.EquipmentMeasure.EquipmentMeasureCategoryID == item.CateID)
                                .Where(i => i.IsPass == false)
                                .Where(i => i.Time <= toDate && i.Time >= fromDate)
                                .Count();

            }
            return results;
        }


    }

    
}
