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
    public class EquipmentProductionErrorSV
    {
        private EquipmentProductionErrorDA ProductionErrorDA = new EquipmentProductionErrorDA();
        public List<EquipmentProductionErrorItem> GetByEquipmentProduction(int page, int pageSize, out int totalCount, int equipmentProductionId)
        {
            var dbo = ProductionErrorDA.Repository;
            var result = ProductionErrorDA.GetQuery(i => i.EquipmentProductionID == equipmentProductionId)
                        .Select(item => new EquipmentProductionErrorItem()
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
                            IsNew = item.IsNew,
                            IsFixed = item.IsFixed,
                            Content = item.Content,
                            Cause = item.Cause,
                            Solution = item.Solution,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Problem = item.Problem,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        public EquipmentProductionErrorItem GetById(int Id)
        {
            var dbo = ProductionErrorDA.Repository;
            var result = ProductionErrorDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentProductionErrorItem()
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
                            IsNew = item.IsNew,
                            IsFixed = item.IsFixed,
                            Content = item.Content,
                            Cause = item.Cause,
                            Solution = item.Solution,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Problem = item.Problem,
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật xử lý sự cố
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentProductionErrorItem item, int userID)
        {
            var error = ProductionErrorDA.GetById(item.ID);
            error.ID = item.ID;
            error.EquipmentProductionID = item.EquipmentProductionID;
            error.HumanDepartmentID = item.HumanDepartment.ID != 0 ? (int?)item.HumanDepartment.ID : null;
            error.HumanEmployeeID = item.HumanEmployee.ID != 0 ? (int?)item.HumanEmployee.ID : null;
            error.Time = item.Time;
            error.IsNew = item.IsNew;
            error.IsFixed = item.IsFixed;
            error.Content = item.Content;
            error.Cause = item.Cause;
            error.Solution = item.Solution;
            error.StartTime = item.StartTime;
            error.EndTime = item.EndTime;
            error.Problem = item.Problem;
            error.UpdateAt = DateTime.Now;
            error.UpdateBy = userID;
            ProductionErrorDA.Save();
        }
        /// <summary>
        /// Thêm mới xử lý sự cố
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentProductionErrorItem item, int userID)
        {
            var error = new EquipmentProductionError()
            {
                EquipmentProductionID = item.EquipmentProductionID,
                Time = item.Time,
                IsNew = item.IsNew,
                IsFixed = item.IsFixed,
                Content = item.Content,
                Cause = item.Cause,
                Solution = item.Solution,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Problem = item.Problem,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            error.HumanDepartmentID = item.HumanDepartment.ID != 0 ? (int?)item.HumanDepartment.ID : null;
            error.HumanEmployeeID = item.HumanEmployee.ID != 0 ? (int?)item.HumanEmployee.ID : null;
            ProductionErrorDA.Insert(error);
            ProductionErrorDA.Save();
            return error.ID;
        }
        /// <summary>
        /// Xóa xử lý sự cố
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionErrorDA.Delete(id);
            ProductionErrorDA.Save();
        }
    }
}
