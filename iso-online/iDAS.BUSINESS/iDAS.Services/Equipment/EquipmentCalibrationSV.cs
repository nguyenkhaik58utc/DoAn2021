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
    public class EquipmentCalibrationSV
    {
        private EquipmentCalibrationDA CalibrationDA = new EquipmentCalibrationDA();
        public List<EquipmentCalibrationItem> GetByEquipmentMeasure(int page, int pageSize, out int totalCount, int equipmentMeasureId)
        {
            var result = CalibrationDA.GetQuery(i => i.EquipmentMeasureID == equipmentMeasureId)
                        .Select(item => new EquipmentCalibrationItem()
                        {
                            ID = item.ID,
                            EquipmentMeasureID = item.EquipmentMeasureID,
                            Name = item.EquipmentMeasure.Name,
                            AccreditationContent = item.AccreditationContent,
                            AccreditationPeriodic = item.AccreditationPeriodic,
                            AccreditationLastTime = item.AccreditationLastTime,
                            AccreditationNextTime = item.AccreditationNextTime,
                            AccreditationBy = item.AccreditationBy,
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
        public List<EquipmentCalibrationItem> GetAll()
        {
            var result = CalibrationDA.GetQuery(i => i.IsPass!=true).Distinct()
                        .Select(item => new EquipmentCalibrationItem()
                        {
                            ID = item.ID,
                            EquipmentMeasureID = item.EquipmentMeasureID,
                            Name = item.EquipmentMeasure.Name,
                            AccreditationContent = item.AccreditationContent,
                            AccreditationPeriodic = item.AccreditationPeriodic,
                            AccreditationLastTime = item.AccreditationLastTime,
                            AccreditationNextTime = item.AccreditationNextTime,
                            AccreditationBy = item.AccreditationBy,
                            IsPass = item.IsPass,
                            Time = item.Time,
                            Note = item.Note,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return result;
        }
        public EquipmentCalibrationItem GetById(int Id)
        {
            var result = CalibrationDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentCalibrationItem()
                        {
                            ID = item.ID,
                            EquipmentMeasureID = item.EquipmentMeasureID,
                            AccreditationContent = item.AccreditationContent,
                            AccreditationPeriodic = item.AccreditationPeriodic,
                            AccreditationLastTime = item.AccreditationLastTime,
                            AccreditationNextTime = item.AccreditationNextTime,
                            AccreditationBy = item.AccreditationBy,
                            IsPass = item.IsPass,
                            Time = item.Time,
                            Note = item.Note,
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật  thiết bị kiểm định
        /// || Author: Thanhnv || CreateDate: 29/06/2015 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentCalibrationItem item, int userID)
        {
            var Measure = CalibrationDA.GetById(item.ID);
            Measure.ID = item.ID;
            Measure.EquipmentMeasureID = item.EquipmentMeasureID;
            Measure.AccreditationContent = item.AccreditationContent;
            Measure.AccreditationPeriodic = item.AccreditationPeriodic;
            Measure.AccreditationLastTime = item.AccreditationLastTime;
            Measure.AccreditationNextTime = item.AccreditationNextTime;
            Measure.AccreditationBy = item.AccreditationBy;
            Measure.IsPass = item.IsPass;
            Measure.Time = item.Time;
            Measure.Note = item.Note;
            Measure.UpdateAt = DateTime.Now;
            Measure.UpdateBy = userID;
            CalibrationDA.Save();
        }
        /// <summary>
        /// Thêm mới  thiết bị kiểm định
        /// || Author: Thanhnv || CreateDate: 29/06/2015 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentCalibrationItem item, int userID)
        {
            var EquipmentMeasure = new EquipmentCalibration()
            {
                EquipmentMeasureID = item.EquipmentMeasureID,
                AccreditationContent = item.AccreditationContent,
                AccreditationPeriodic = item.AccreditationPeriodic,
                AccreditationLastTime = item.AccreditationLastTime,
                AccreditationNextTime = item.AccreditationNextTime,
                AccreditationBy = item.AccreditationBy,
                IsPass = item.IsPass,
                Time = item.Time,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CalibrationDA.Insert(EquipmentMeasure);
            CalibrationDA.Save();
            return EquipmentMeasure.ID;
        }
        /// <summary>
        /// Xóa thiết bị kiểm định
        /// || Author: Thanhnv || CreateDate: 29/06/2015 
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CalibrationDA.Delete(id);
            CalibrationDA.Save();
        }
    }
}
