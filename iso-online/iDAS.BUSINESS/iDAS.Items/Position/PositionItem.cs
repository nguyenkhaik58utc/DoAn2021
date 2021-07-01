using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items.Position
{
    public class PositionItem
    {
        public int ID { get; set; }
        public string PositionCode { get; set; }
        public string PositionName { get; set; }
        public int? ManagementLevelID { get; set; }
        public string ManagementLevelName { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }

        public PositionItem()
        {
        }

        public PositionItem(int iD, string positionCode, string positionName, int? managementLevelID, bool isDelete, bool isActive, int? createBy, DateTime? createAt, int? updateBy, DateTime? updateAt)
        {
            ID = iD;
            PositionCode = positionCode;
            PositionName = positionName;
            ManagementLevelID = managementLevelID;
            IsDelete = isDelete;
            IsActive = isActive;
            CreateBy = createBy;
            CreateAt = createAt;
            UpdateBy = updateBy;
            UpdateAt = updateAt;
        }
    }
}
