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
    public class EquipmentProductionMaintainSV
    {
        private EquipmentProductionMaintainDA ProductionMaintainDA = new EquipmentProductionMaintainDA();
        public List<EquipmentProductionMaintainItem> GetByEquipmentProduction(int page, int pageSize, out int totalCount, int equipmentProductionId)
        {
            var listMaintain = ProductionMaintainDA.GetQuery(i => i.EquipmentProductionID == equipmentProductionId)
                        .Select(item => new EquipmentProductionMaintainItem()
                        {
                            ID = item.ID,
                            EquipmentProductionID = item.EquipmentProductionID,
                            HumanDepartment = new HumanDepartmentViewItem()
                                    {
                                        ID = item.HumanDepartmentID == null ? 0 : (int)item.HumanDepartmentID,
                                        Name = item.HumanDepartmentID == null ? "" : item.HumanDepartment.Name
                                    },
                            PlanTime = item.PlanTime,
                            RealTime = item.RealTime,
                            IsComplete = item.IsComplete,
                            Content = item.Content,
                            Note = item.Note,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            var yearMaintain = listMaintain.GroupBy(i => i.Year);
            var result = new List<EquipmentProductionMaintainItem>();
            foreach (var maintains in yearMaintain)
            {
                if (maintains.Count() > 0)
                {
                    int i = 0;
                    foreach (var maintain in maintains)
                    {
                        i++;
                        maintain.Number = i;
                        result.Add(maintain);
                    }
                }
            }
            return result;
        }
        public EquipmentProductionMaintainItem GetById(int Id)
        {
            var result = ProductionMaintainDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentProductionMaintainItem()
                        {
                            ID = item.ID,
                            EquipmentProductionID = item.EquipmentProductionID,
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID == null ? 0 : (int)item.HumanDepartmentID,
                                Name = item.HumanDepartmentID == null ? "" : item.HumanDepartment.Name
                            },
                            PlanTime = item.PlanTime,
                            RealTime = item.RealTime,
                            IsComplete = item.IsComplete,
                            Content = item.Content,
                            Note = item.Note
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật bảo dưỡng thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentProductionMaintainItem item, int userID)
        {
            var production = ProductionMaintainDA.GetById(item.ID);
            production.ID = item.ID;
            production.PlanTime = item.PlanTime;
            production.Content = item.Content;
            production.UpdateAt = DateTime.Now;
            production.UpdateBy = userID;
            ProductionMaintainDA.Save();
        }
        public void UpdateReal(EquipmentProductionMaintainItem item, int userID)
        {
            var production = ProductionMaintainDA.GetById(item.ID);
            production.ID = item.ID;
            production.HumanDepartmentID = item.HumanDepartment.ID;
            production.RealTime = item.RealTime;
            production.Note = item.Note;
            production.IsComplete = item.IsComplete;
            production.UpdateAt = DateTime.Now;
            production.UpdateBy = userID;
            ProductionMaintainDA.Save();
        }
        /// <summary>
        /// Thêm mới bảo dưỡng thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentProductionMaintainItem item, int userID)
        {
            var EquipmentProduction = new EquipmentProductionMaintain()
            {
                EquipmentProductionID = item.EquipmentProductionID,
                PlanTime = item.PlanTime,
                Content = item.Content,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ProductionMaintainDA.Insert(EquipmentProduction);
            ProductionMaintainDA.Save();
            return EquipmentProduction.ID;
        }
        /// <summary>
        /// Xóabảo dưỡng thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionMaintainDA.Delete(id);
            ProductionMaintainDA.Save();
        }
    }
}
