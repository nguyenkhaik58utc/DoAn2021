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
    public class EquipmentMeasureCalibrationResultSV
    {
        private EquipmentMeasureCalibrationResultDA MeasureCalibrationDA = new EquipmentMeasureCalibrationResultDA();
        public List<EquipmentMeasureCalibrationResultItem> GetByMeasureCalibration(int page, int pageSize, out int totalCount, int calibrationId)
        {
            var dbo = MeasureCalibrationDA.Repository;
            var result = MeasureCalibrationDA.GetQuery(i => i.EquipmentMeasureCalibrationID == calibrationId)
                        .Select(item => new EquipmentMeasureCalibrationResultItem()
                        {
                            ID = item.ID,
                            EquipmentMeasureCalibrationID = item.EquipmentMeasureCalibrationID,
                            EquipmentMeasureCalibrationName = item.EquipmentMeasureCalibration.EquipmentMeasure.Name,
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
        public EquipmentMeasureCalibrationResultItem GetById(int Id)
        {
            var result = MeasureCalibrationDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentMeasureCalibrationResultItem()
                        {
                            ID = item.ID,
                            EquipmentMeasureCalibrationID = item.EquipmentMeasureCalibrationID,
                            IsPass  = item.IsPass,
                            Time = item.Time,
                            Note = item.Note,
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật  kết quả hiệu chuẩn
        /// || Author: Thanhnv || CreateDate: 29/06/2015 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentMeasureCalibrationResultItem item, int userID)
        {
            var Measure = MeasureCalibrationDA.GetById(item.ID);
            Measure.ID = item.ID;
            Measure.EquipmentMeasureCalibrationID = item.EquipmentMeasureCalibrationID;
            Measure.IsPass = item.IsPass;
            Measure.Time = item.Time;
            Measure.Note = item.Note;
            Measure.UpdateAt = DateTime.Now;
            Measure.UpdateBy = userID;
            MeasureCalibrationDA.Save();
        }
        /// <summary>
        /// Thêm mới  kết quả hiệu chuẩn
        /// || Author: Thanhnv || CreateDate: 29/06/2015 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentMeasureCalibrationResultItem item, int userID)
        {
            var EquipmentMeasure = new EquipmentMeasureCalibrationResult()
            {
                EquipmentMeasureCalibrationID = item.EquipmentMeasureCalibrationID,
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
        /// Xóa kết quả hiệu chuẩn
        /// || Author: Thanhnv || CreateDate: 29/06/2015 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            MeasureCalibrationDA.Delete(id);
            MeasureCalibrationDA.Save();
        }
    }
}
