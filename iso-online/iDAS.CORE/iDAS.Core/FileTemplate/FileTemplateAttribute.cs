using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Core
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class FileTemplateAttribute : Attribute
    {
        public FileTemplateAttribute(FileTemplateType type, string name = "")
        {
            Type = type;
            Name = name;
        }
        public FileTemplateType Type { get; set; }
        public String Name { get; set; }
    }
}
