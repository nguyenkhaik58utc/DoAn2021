using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DocumentDistributionItem
    {
        public int ID { get; set; }
        public int DocumentDistributionID { get; set; }
        public int ParentID { get; set; }
        public int? DepartmentID { get; set; }
        public string Department { get; set; }
        public int? DocumentID { get; set; }
        public string DocumentDeptName { get; set; }
        public string Name { get; set; }
        public bool FormH { get; set; }
        public bool FormS { get; set; }
        public bool? FormHO { get; set; }
        public bool? FormSO { get; set; }
        public int Number { get; set; }
        public string NoteIssues { get; set; }
        public string NoteObs { get; set; }
        public int NumberObsolete { get; set; }
        public HumanEmployeeViewItem EmployeeInfo { get; set; }
        public Nullable<System.DateTime> IssuedDate { get; set; }//Ngày ban hành
        public Nullable<System.DateTime> ObsoleteDate { get; set; }//Ngày thu hồi
        public Nullable<System.DateTime> DistributionDate { get; set; }//Ngày phân phối
        public int? IssuedBy { get; set; }
        public int? ObsoleteBy { get; set; }
        public string IssuedName { get; set; }
        public string ObsoleteName { get; set; }
        public string IssuedTypeDisp { get; set; }
        public string ObsoleteTypeDisp { get; set; }
        public string Avatar { get; set; }
        public string Position { get; set; }
        public int? UpdateBy { get; set; }
        public int? CreateBy { get; set; }
        public HumanDepartmentViewItem Departments { get; set; }
        public List<Guid> AttachmentFileIDs { get; set; }
    }
}
