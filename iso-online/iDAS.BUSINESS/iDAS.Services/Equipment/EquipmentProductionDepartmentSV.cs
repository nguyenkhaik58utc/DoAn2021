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
    public class EquipmentProductionDepartmentSV
    {
        private EquipmentProductionDepartmentDA ProductionDepartmentDA = new EquipmentProductionDepartmentDA();
        public List<EquipmentProductionDepartmentItem> GetByEquipmentProduction(int page, int pageSize, out int totalCount, int equipmentProductionId)
        {
            var result = ProductionDepartmentDA.GetQuery(i => i.EquipmentProductionID == equipmentProductionId)
                        .Select(item => new EquipmentProductionDepartmentItem()
                        {
                            ID = item.ID,
                            EquipmentProductionID = item.EquipmentProductionID,
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
        public EquipmentProductionDepartmentItem GetById(int Id)
        {
            var result = ProductionDepartmentDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentProductionDepartmentItem()
                        {
                            ID = item.ID,
                            EquipmentProductionID = item.EquipmentProductionID,
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
        public void Update(EquipmentProductionDepartmentItem item, int userID)
        {
            var production = ProductionDepartmentDA.GetById(item.ID);
            production.ID = item.ID;
            production.EquipmentProductionID = item.EquipmentProductionID;
            production.EndTime = item.EndTime;
            production.HumanDepartmentID = item.HumanDepartment.ID;
            production.IsUsing = true;
            production.Note = item.Note;
            production.StartTime = item.StartTime;
            production.UpdateAt = DateTime.Now;
            production.UpdateBy = userID;
            ProductionDepartmentDA.Save();
        }
        /// <summary>
        /// Thêm mới  phân phối thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentProductionDepartmentItem item, int userID)
        {
            var EquipmentProduction = new EquipmentProductionDepartment()
            {
                EquipmentProductionID = item.EquipmentProductionID,
                EndTime = item.EndTime,
                HumanDepartmentID = item.HumanDepartment.ID,
                IsUsing = true,
                Note = item.Note,
                StartTime = item.StartTime,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ProductionDepartmentDA.Insert(EquipmentProduction);
            ProductionDepartmentDA.Save();
            return EquipmentProduction.ID;
        }
        /// <summary>
        /// Xóa phân phối thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionDepartmentDA.Delete(id);
            ProductionDepartmentDA.Save();
        }
        /// <summary>
        ///Thu hồi thiết bị
        /// || Author: Thanhnv || CreateDate: 29/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Destroy(int productionId, int userID)
        {
            var usingProductions = ProductionDepartmentDA.GetQuery(i => i.EquipmentProductionID == productionId && i.IsUsing).ToList();
            if (usingProductions != null && usingProductions.Count > 0)
            {
                foreach (var usingProduction in usingProductions)
                {
                    usingProduction.IsUsing = false;
                    usingProduction.UpdateAt = DateTime.Now;
                    usingProduction.UpdateBy = userID;
                }
                ProductionDepartmentDA.Save();
            }
        }
        public bool ExitsDepartmentUsing(int productionId)
        {
            var usingProductions = ProductionDepartmentDA.GetQuery(i => i.EquipmentProductionID == productionId && i.IsUsing);
            if (usingProductions != null && usingProductions.Count() > 0) return true;
            return false;
        }
    }
}
