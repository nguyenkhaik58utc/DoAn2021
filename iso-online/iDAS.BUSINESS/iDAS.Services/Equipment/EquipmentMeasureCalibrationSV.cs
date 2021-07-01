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
    public class EquipmentMeasureCalibrationSV
    {
        private EquipmentMeasureCalibrationDA MeasureCalibrationDA = new EquipmentMeasureCalibrationDA();
        public List<EquipmentMeasureCalibrationItem> GetByEquipmentMeasure(int page, int pageSize, out int totalCount, int equipmentMeasureId)
        {
            var dbo = MeasureCalibrationDA.Repository;
            var result = MeasureCalibrationDA.GetQuery(i => i.EquipmentMeasureID == equipmentMeasureId)
                        .Select(item => new EquipmentMeasureCalibrationItem()
                        {
                            ID = item.ID,
                            EquipmentMeasureID = item.EquipmentMeasureID,
                            EquipmentCalibrationID = item.EquipmentCalibrationID,
                            EquipmentCalibrationName = item.EquipmentCalibration.EquipmentMeasure.Name,
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.HumanEmployeeID != null ? item.HumanEmployee.ID : 0,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault(),
                                Role = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.Name).FirstOrDefault(),
                                Department = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                            },
                            IsPass = item.IsPass,
                            Time = item.Time,
                            Note = item.Note,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        public EquipmentMeasureCalibrationItem GetById(int Id)
        {
            var dbo = MeasureCalibrationDA.Repository;
            var result = MeasureCalibrationDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentMeasureCalibrationItem()
                        {
                            ID = item.ID,
                            EquipmentMeasureID = item.EquipmentMeasureID,
                            EquipmentCalibrationID = item.EquipmentCalibrationID,
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.HumanEmployeeID != null ? item.HumanEmployee.ID : 0,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault(),
                                Role = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.Name).FirstOrDefault(),
                                Department = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                            },
                            IsPass = item.IsPass,
                            Time = item.Time,
                            Note = item.Note,
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật  thiết bị hiệu chuẩn
        /// || Author: Thanhnv || CreateDate: 29/06/2015 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentMeasureCalibrationItem item, int userID)
        {
            var measure = MeasureCalibrationDA.GetById(item.ID);
            measure.ID = item.ID;
            measure.EquipmentMeasureID = item.EquipmentMeasureID;
            measure.EquipmentCalibrationID = item.EquipmentCalibrationID;
            measure.Time = item.Time;
            measure.IsPass = item.IsPass;
            measure.Note = item.Note;
            measure.UpdateAt = DateTime.Now;
            measure.UpdateBy = userID;
            MeasureCalibrationDA.Save();
        }
        /// <summary>
        /// Thêm mới  thiết bị hiệu chuẩn
        /// || Author: Thanhnv || CreateDate: 29/06/2015 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentMeasureCalibrationItem item, int userID)
        {
            var EquipmentMeasure = new EquipmentMeasureCalibration()
            {
                EquipmentMeasureID = item.EquipmentMeasureID,
                EquipmentCalibrationID = item.EquipmentCalibrationID,
                HumanEmployeeID = item.HumanEmployee.ID,
                IsPass = item.IsPass,
                Time = item.Time,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            MeasureCalibrationDA.Insert(EquipmentMeasure);
            MeasureCalibrationDA.Save();
            return EquipmentMeasure.ID;
        }
        /// <summary>
        /// Xóa thiết bị hiệu chuẩn
        /// || Author: Thanhnv || CreateDate: 29/06/2015 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            MeasureCalibrationDA.Delete(id);
            MeasureCalibrationDA.Save();
        }
         public List<EquipmentCalibrationItem> GetCalibration()
        {
            EquipmentCalibrationSV equipmentCalibrationSV = new EquipmentCalibrationSV();
            return equipmentCalibrationSV.GetAll();
        }
    }
}
