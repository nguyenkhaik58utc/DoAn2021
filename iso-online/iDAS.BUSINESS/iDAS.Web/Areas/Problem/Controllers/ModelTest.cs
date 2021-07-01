using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Problem.Controllers
{
    public class ModelTest
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime date { get; set; }
        public int status { get; set; }

        public ModelTest(int iD, string type, string description, DateTime date, int status)
        {
            ID = iD;
            Type = type;
            Description = description;
            this.date = date;
            this.status = status;
        }
    }
}