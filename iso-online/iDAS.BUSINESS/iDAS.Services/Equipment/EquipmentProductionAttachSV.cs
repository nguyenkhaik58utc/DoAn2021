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
    public class EquipmentProductionAttachSV
    {
        private EquipmentProductionAttachDA ProductionAttachDA = new EquipmentProductionAttachDA();
        public List<EquipmentProductionAttachItem> GetByEquipmentProduction(int page, int pageSize, out int totalCount, int equipmentProductionId)
        {
            var result = ProductionAttachDA.GetQuery(i => i.EquipmentProductionID == equipmentProductionId)
                        .Select(item => new EquipmentProductionAttachItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            SerialNumber = item.SerialNumber,
                            MadeIn = item.MadeIn,
                            MadeYear = item.MadeYear,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            IsError = item.IsError,
                            IsFixed = item.IsFixed,
                            IsUsing = item.IsUsing,
                            CreateAt = item.CreateAt
                        })
                        .OrderBy(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return result;
        }
        public EquipmentProductionAttachItem GetById(int Id)
        {
            var result = ProductionAttachDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentProductionAttachItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            SerialNumber = item.SerialNumber,
                            MadeIn = item.MadeIn,
                            EquipmentProductionID = item.EquipmentProductionID,
                            MadeYear = item.MadeYear,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            IsError = item.IsError,
                            IsFixed = item.IsFixed,
                            IsUsing = item.IsUsing,
                            Note = item.Note,
                            Specifications = item.Specifications,
                            Features = item.Features,
                            CreateAt = item.CreateAt
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật thiết bị đi kèm
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentProductionAttachItem item, int userID)
        {
            var Attach = ProductionAttachDA.GetById(item.ID);        
            Attach.EquipmentProductionID = item.EquipmentProductionID;
            Attach.Code = item.Code;
            Attach.Name = item.Name;
            Attach.SerialNumber = item.SerialNumber;
            Attach.MadeIn = item.MadeIn;
            Attach.MadeYear = item.MadeYear;
            Attach.IsActive = item.IsActive;
            Attach.IsDelete = item.IsDelete;
            Attach.IsError = item.IsError;
            Attach.IsFixed = item.IsFixed;
            Attach.IsUsing = item.IsUsing;
            Attach.Note = item.Note;
            Attach.Specifications = item.Specifications;
            Attach.Features = item.Features;
            Attach.UpdateAt = DateTime.Now;
            Attach.UpdateBy = userID;
            ProductionAttachDA.Save();
        }
        /// <summary>
        /// Thêm mới thiết bị đi kèm
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentProductionAttachItem item, int userID)
        {
            var EquipmentProduction = new EquipmentProductionAttach()
            {
                EquipmentProductionID = item.EquipmentProductionID,
                Code = item.Code,
                Name = item.Name,
                SerialNumber = item.SerialNumber,
                MadeIn = item.MadeIn,
                MadeYear = item.MadeYear,
                IsActive = item.IsActive,
                IsDelete = item.IsDelete,
                IsError = item.IsError,
                IsFixed = item.IsFixed,
                IsUsing = item.IsUsing,
                Note = item.Note,
                Specifications = item.Specifications,
                Features = item.Features,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ProductionAttachDA.Insert(EquipmentProduction);
            ProductionAttachDA.Save();
            return EquipmentProduction.ID;
        }
        /// <summary>
        /// Xóathiết bị đi kèm
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionAttachDA.Delete(id);
            ProductionAttachDA.Save();
        }
    }
}
