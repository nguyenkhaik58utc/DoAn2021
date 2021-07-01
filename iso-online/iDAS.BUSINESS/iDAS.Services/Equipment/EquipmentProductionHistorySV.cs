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
    public class EquipmentProductionHistorySV
    {
        private EquipmentProductionHistoryDA ProductionHistoryDA = new EquipmentProductionHistoryDA();
        public List<EquipmentProductionHistoryItem> GetByEquipmentProduction(int page, int pageSize, out int totalCount, int equipmentProductionId)
        {
            var dbo = ProductionHistoryDA.Repository;
            var result = ProductionHistoryDA.GetQuery(i => i.EquipmentProductionID == equipmentProductionId)
                        .Select(item => new EquipmentProductionHistoryItem()
                        {
                            ID = item.ID,
                            EquipmentProductionID = item.EquipmentProductionID,
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID == null ? 0 : (int)item.HumanDepartmentID,
                                Name = item.HumanDepartmentID == null ? "" : item.HumanDepartment.Name
                            },
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.HumanEmployeeID != null ? item.HumanEmployee.ID : 0,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault(),
                                Role = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.Name).FirstOrDefault(),
                                Department = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                            },
                            Time = item.Time,
                            IsUsing = item.IsUsing,
                            IsError = item.IsError,
                            IsFixed = item.IsFixed,
                            IsMaintain = item.IsMaintain,
                            Note = item.Note,
                            IsCalibration = item.IsCalibration,
                            IsAccreditation = item.IsAccreditation,
                            IsNotUse = item.IsNotUse,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        public EquipmentProductionHistoryItem GetById(int Id)
        {
            var dbo = ProductionHistoryDA.Repository;
            var result = ProductionHistoryDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentProductionHistoryItem()
                        {
                            ID = item.ID,
                            EquipmentProductionID = item.EquipmentProductionID,
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID == null ? 0 : (int)item.HumanDepartmentID,
                                Name = item.HumanDepartmentID == null ? "" : item.HumanDepartment.Name
                            },
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.HumanEmployeeID != null ? item.HumanEmployee.ID : 0,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault(),
                                Role = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.Name).FirstOrDefault(),
                                Department = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                            },
                            Time = item.Time,
                            IsUsing = item.IsUsing,
                            IsError = item.IsError,
                            IsFixed = item.IsFixed,
                            IsMaintain = item.IsMaintain,
                            Note = item.Note,
                            IsCalibration = item.IsCalibration,
                            IsAccreditation = item.IsAccreditation,
                            IsNotUse = item.IsNotUse
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật theo dõi thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentProductionHistoryItem item, int userID)
        {
            var history = ProductionHistoryDA.GetById(item.ID);
            history.ID = item.ID;
            history.EquipmentProductionID = item.EquipmentProductionID;
            history.HumanDepartmentID = item.HumanDepartment.ID != 0 ? (int?)item.HumanDepartment.ID : null;
            history.HumanEmployeeID = item.HumanEmployee.ID != 0 ? (int?)item.HumanEmployee.ID : null;
            history.Time = item.Time;
            history.IsUsing = item.IsUsing;
            history.IsError = item.IsError;
            history.IsFixed = item.IsFixed;
            history.IsMaintain = item.IsMaintain;
            history.Note = item.Note;
            history.IsCalibration = item.IsCalibration;
            history.IsAccreditation = item.IsAccreditation;
            history.IsNotUse = item.IsNotUse;
            history.UpdateAt = DateTime.Now;
            history.UpdateBy = userID;
            ProductionHistoryDA.Save();
        }
        /// <summary>
        /// Thêm mới theo dõi thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentProductionHistoryItem item, int userID)
        {
            var history = new EquipmentProductionHistory()
            {
                EquipmentProductionID = item.EquipmentProductionID,
                Time = item.Time,
                IsUsing = item.IsUsing,
                IsError = item.IsError,
                IsFixed = item.IsFixed,
                IsMaintain = item.IsMaintain,
                Note = item.Note,
                IsCalibration = item.IsCalibration,
                IsAccreditation = item.IsAccreditation,
                IsNotUse = item.IsNotUse,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            history.HumanDepartmentID = item.HumanDepartment.ID != 0 ? (int?)item.HumanDepartment.ID : null;
            history.HumanEmployeeID = item.HumanEmployee.ID != 0 ? (int?)item.HumanEmployee.ID : null;
            ProductionHistoryDA.Insert(history);
            ProductionHistoryDA.Save();
            return history.ID;
        }
        /// <summary>
        /// Xóatheo dõi thiết bị
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionHistoryDA.Delete(id);
            ProductionHistoryDA.Save();
        }
    }
}
