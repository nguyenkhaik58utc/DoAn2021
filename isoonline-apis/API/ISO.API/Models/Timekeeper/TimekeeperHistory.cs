using System;

namespace ISO.API.Models
{
    public class TimekeeperHistory
    {
        public long ID { set; get; }
        public int TimekeeperID { set; get; }
        public string Code { set; get; }
        public DateTime DateTimekeeping { set; get; }

    }
}
