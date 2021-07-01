using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class StatisticalQualityPlanItem
    {
        public int DepartmentID { get; set; }
        //Tên nhóm
        public string Name { get; set; }
        // Tổng số
        public int Total { get; set; }
        // Mới
        public int New { get; set; }
        // chờ duyệt
        public int Wait { get; set; }
        // Sửa đổi
        public int Edit { get; set; }
        // Không duyệt
        public int NotApproval { get; set; }
        // Đang thực hiện
        public int Performing { get; set; }
        // Kết thúc
        public int EndTotal { get; set; }
        //Hoàn thành
        public int Complete { get; set; }
        // Đạt
        public int Ok { get; set; }
        // Không đạt
        public int NotOk { get; set; }
         // chưa hoàn thành
        public int NotComplete { get; set; }

        public bool IsLeaf { get; set; }
    }
}
