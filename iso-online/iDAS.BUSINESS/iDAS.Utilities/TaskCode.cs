using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Utilities
{
    public enum ETaskCode
    {
        Document,
        Recruitment,       
        Other
    }
    
    public static class TaskCode 
    {
        public static string Text(this ETaskCode e)
        { 
            return getText(e.Code());
        }
        public static string Code(this ETaskCode e)
        {
            return e.ToString();
        }
        public static string Text(this string code) {
            return getText(code);
        }
        private static string getText(string code)
        {
            string result = string.Empty;
            if (code == ETaskCode.Document.Code()) return  result = "Tài liệu";
            if (code == ETaskCode.Recruitment.Code()) return result = "Tuyển dụng";
            if (code == ETaskCode.Other.Code()) return result = "Công việc khác";
            return result;
        }
        
    }
}