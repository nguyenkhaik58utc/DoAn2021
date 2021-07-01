using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerSurveyQuestionItem
    {
        public int ID { get; set; }
        public int SurveyID { get; set; }
        public string Name { get; set; }
        public bool IsCategory { get; set; }
        public bool IsMulti { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public int ParentID { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsRoot { get; set; }
        public string ActionForm { get; set; }
        public int GroupID { get; set; }
        public int CustomerID { get; set; }
        public int? ResultID { get; set; }
        public bool IsSelect { get; set; }
    }
    public class TreeSurveyQuestionItem
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int SurveyID { get; set; }
        public string Name { get; set; }
        public bool IsCategory { get; set; }
        public bool IsUse { get; set; }
        public bool Leaf { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
    }
    public class TreeCusotmerSurveyItem : TreeSurveyQuestionItem
    {
        public int CustomerID { get; set; }
        public int? ResultID { get; set; }
        public bool IsSelect { get; set; }
    }
    public class CustomerAnswerItem
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int SurveyID { get; set; }
        public string Name { get; set; }
        public int Sum { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
    }
#region Thiết lập đối tượng để gửi mail
    public class SurveyMailObject
    {
        public List<CustomerDetail> Customers { get; set; }
        public List<SurveyMailQuestion> MailContent { get; set; }
    }
    public class SurveyMailQuestion
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsMulti { get; set; }
        public List<SurveyMailAnswer> Answer { get; set; }
    }
    public class SurveyMailAnswer
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
#endregion
}