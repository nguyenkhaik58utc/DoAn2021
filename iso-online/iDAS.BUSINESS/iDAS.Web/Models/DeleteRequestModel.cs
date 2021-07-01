using System;

namespace iDAS.Web.Models
{
    public class DeleteRequestModel
    {
        public int ID { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}