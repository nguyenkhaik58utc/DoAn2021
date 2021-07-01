namespace iDAS.Web.Areas.Document.Models
{
    public class DocumentReqModel
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public int folderID { get; set; }

        public int userID { get; set; }
        public int employeeID { get; set; }
        public bool type { get; set; }
    }
}
