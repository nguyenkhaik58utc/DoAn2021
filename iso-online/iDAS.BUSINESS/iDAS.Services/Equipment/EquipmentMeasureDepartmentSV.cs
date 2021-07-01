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
    public class EquipmentMeasureDepartmentSV
    {
        private EquipmentMeasureDepartmentDA MeasureDepartmentDA = new EquipmentMeasureDepartmentDA();
        public List<EquipmentMeasureDepartmentItem> GetByEquipmentMeasure(int page, int pageSize, out int totalCount, int equipmentMeasureId)
        {
            var result = MeasureDepartmentDA.GetQuery(i => i.EquipmentMeasureID == equipmentMeasureId)
                        .Select(item => new EquipmentMeasureDepartmentItem()
                        {
                            ID = item.ID,
                            EquipmentMeasureID = item.EquipmentMeasureID,
                            EndTime = item.EndTime,
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID,
                                Name = item.HumanDepartment.Name
                            },
                            IsUsing = item.IsUsing,
                            Note = item.Note,
                            StartTime = item.StartTime,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        public EquipmentMeasureDepartmentItem GetById(int Id)
        {
            var result = MeasureDepartmentDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentMeasureDepartmentItem()
                        {
                            ID = item.ID,
                            EquipmentMeasureID = item.EquipmentMeasureID,
                            EndTime = item.EndTime,
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID,
                                Name = item.HumanDepartment.Name
                            },
                            IsUsing = item.IsUsing,
                            Note = item.Note,
                            StartTime = item.StartTime,
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật  phân phối thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentMeasureDepartmentItem item, int userID)
        {
            var dbo = MeasureDepartmentDA.Repository;
            var Measure = MeasureDepartmentDA.GetById(item.ID);
            Measure.ID = item.ID;
            Measure.EquipmentMeasureID = item.EquipmentMeasureID;
            Measure.EndTime = item.EndTime;
            Measure.HumanDepartmentID = item.HumanDepartment.ID;
            Measure.IsUsing = true;
            Measure.Note = item.Note;
            Measure.StartTime = item.StartTime;
            Measure.UpdateAt = DateTime.Now;
            Measure.UpdateBy = userID;
            MeasureDepartmentDA.Save();
           var m = dbo.EquipmentMeasures.FirstOrDefault(t => t.ID == item.EquipmentMeasureID);
           m.IsUsing = true;
           dbo.SaveChanges();
        }
        /// <summary>
        /// Thêm mới  phân phối thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentMeasureDepartmentItem item, int userID)
        {
            var dbo = MeasureDepartmentDA.Repository;
            var EquipmentMeasure = new EquipmentMeasureDepartment()
            {
                EquipmentMeasureID = item.EquipmentMeasureID,
                EndTime = item.EndTime,
                HumanDepartmentID = item.HumanDepartment.ID,
                IsUsing = true,
                Note = item.Note,
                StartTime = item.StartTime,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            MeasureDepartmentDA.Insert(EquipmentMeasure);
            MeasureDepartmentDA.Save();
            var m = dbo.EquipmentMeasures.FirstOrDefault(t => t.ID == item.EquipmentMeasureID);
            m.IsUsing = true;
            dbo.SaveChanges();
            return EquipmentMeasure.ID;
        }
        /// <summary>
        /// Xóa phân phối thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            var dbo = MeasureDepartmentDA.Repository;
            var usingMeasures = MeasureDepartmentDA.GetQuery(i => i.ID == id).FirstOrDefault();
            if (usingMeasures.IsUsing)
            {
                var m = dbo.EquipmentMeasures.FirstOrDefault(t => t.ID == usingMeasures.EquipmentMeasureID);
                m.IsUsing = false;
                dbo.SaveChanges();
            }
            MeasureDepartmentDA.Delete(id);
            MeasureDepartmentDA.Save();
           
        }
        /// <summary>
        ///Thu hồi thiết bị
        /// || Author: Thanhnv || CreateDate: 29/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Destroy(int MeasureId, int userID)
        {
            var dbo = MeasureDepartmentDA.Repository;
            var usingMeasures = MeasureDepartmentDA.GetQuery(i => i.EquipmentMeasureID == MeasureId && i.IsUsing).ToList();
            if (usingMeasures != null && usingMeasures.Count > 0)
            {
                foreach (var usingMeasure in usingMeasures)
                {
                    usingMeasure.IsUsing = false;
                    usingMeasure.UpdateAt = DateTime.Now;
                    usingMeasure.UpdateBy = userID;
                }
                var m = dbo.EquipmentMeasures.FirstOrDefault(t => t.ID == MeasureId);
                m.IsUsing = false;
                dbo.SaveChanges();
                MeasureDepartmentDA.Save();
            }
        }

        public bool ExitsDepartmentUsing(int measureId)
        {
            var usingProductions = MeasureDepartmentDA.GetQuery(i => i.EquipmentMeasureID == measureId && i.IsUsing);
            if (usingProductions != null && usingProductions.Count() > 0) return true;
            return false;
        }
    }
}
