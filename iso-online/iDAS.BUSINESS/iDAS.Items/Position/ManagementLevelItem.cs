using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items.Position
{
    public class ManagementLevelItem
    {
        public int ID { get; set; }
        public string ManagementLevelCode { get; set; }
        public string ManagementLevelName { get; set; }
        public int Rank { get; set; }
        public bool IsDelete { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }

        public ManagementLevelItem()
        {
        }

        public ManagementLevelItem(int iD, string positionCode, string positionName, int rank, bool isDelete, int? createBy, DateTime? createAt, int? updateBy, DateTime? updateAt)
        {
            ID = iD;
            ManagementLevelCode = positionCode;
            ManagementLevelName = positionName;
            Rank = rank;
            IsDelete = isDelete;
            CreateBy = createBy;
            CreateAt = createAt;
            UpdateBy = updateBy;
            UpdateAt = updateAt;
        }
    }
}
