//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iDAS.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class HumanQuestion
    {
        public HumanQuestion()
        {
            this.HumanAnswers = new HashSet<HumanAnswer>();
            this.HumanResultQuestions = new HashSet<HumanResultQuestion>();
        }
    
        public int ID { get; set; }
        public Nullable<int> HumanCategoryQuestionID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsUse { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    
        public virtual ICollection<HumanAnswer> HumanAnswers { get; set; }
        public virtual HumanCategoryQuestion HumanCategoryQuestion { get; set; }
        public virtual ICollection<HumanResultQuestion> HumanResultQuestions { get; set; }
    }
}
