using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanOrganizationItem
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
    /// <summary>
    /// Position and Group Item
    /// Author: Thanhnv
    /// Date: 09/12/2014
    /// </summary>
    public class HumanGroupPositionItem
    {
        public int? ID { get; set; }
        public string GroupName { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
    }
}
