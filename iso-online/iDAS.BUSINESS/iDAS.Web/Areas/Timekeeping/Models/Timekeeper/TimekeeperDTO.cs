﻿namespace iDAS.Web.Areas.Timekeeping.Models
{
    public class TimekeeperDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public int Status { get; set; }
        public int Post { get; set; }
        public string Serial { get; set; }
        public string StatusText { get; set; }
    }
}
