using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Areas.Human.Models
{
    public class GroupCheckBoxBO
    {
        // thông tin chung
        public List<CheckBoxBO> groupCV { get; set; }
        // chính trị
        public List<CheckBoxBO> groupPolitic { get; set; }
        public List<CheckBoxBO> groupWorkExp { get; set; }
        public List<CheckBoxBO> groupTraining { get; set; }
        public List<CheckBoxBO> groupDiploma { get; set; }
        public List<CheckBoxBO> groupCertificate { get; set; }
        public List<CheckBoxBO> groupReward { get; set; }

        public GroupCheckBoxBO()
        {
            groupCV = new List<CheckBoxBO>();
            groupPolitic = new List<CheckBoxBO>();
            groupWorkExp = new List<CheckBoxBO>();
            groupTraining = new List<CheckBoxBO>();
            groupDiploma = new List<CheckBoxBO>();
            groupCertificate = new List<CheckBoxBO>();
            groupReward = new List<CheckBoxBO>();
        }
    }

    public class CheckBoxBO
    {
        public string FieldLabel { get; set; }
        public bool FieldValue { get; set; }
        public string Title { get; set; }

        public CheckBoxBO()
        {

        }
        public CheckBoxBO(string FieldLabel, bool FieldValue, string Title, string Data = null)
        {
            this.FieldLabel = FieldLabel;
            this.FieldValue = FieldValue;
            this.Title = Title;
        }
    }
}