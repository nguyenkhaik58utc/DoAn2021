using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iDAS.Core
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ExportAttribute : Attribute
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Query { get; set; }

    }
    public class ExportField
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Code { get; set; }
    }
    public class ExportTable : ExportField
    {
        public string Query { get; set; }
        public List<ExportField> Fields { get; set; }
    }
    public class Export
    {
        public List<ExportTable> GetExportTables(Type type)
        {
            var result = type.Assembly.GetTypes()
                    .Where(i => i.IsClass && i.Namespace == type.Namespace)
                    .Where(i => i.GetCustomAttributes<ExportAttribute>(false).Count() > 0)
                    .Select(i =>
                    {
                        var f = i.GetCustomAttribute<ExportAttribute>(false);
                        return new ExportTable()
                        {
                            Code = i.Name,
                            Name = f.Name,
                            Id = f.Id ?? i.Name,
                            Query = f.Query,
                            Fields = GetExportFields(i)
                        };
                    }).ToList();

            return result;
        }
        public List<ExportField> GetExportFields(Type type)
        {
            var result = type.GetMembers()
                    .Where(i => i.GetCustomAttributes<ExportAttribute>(false).Count() > 0)
                    .Select(i =>
                    {
                        var f = i.GetCustomAttribute<ExportAttribute>(false);
                        return new ExportField()
                        {
                            Code = i.Name,
                            Name = f.Name,
                            Id = f.Id ?? i.Name,
                        };
                    }).ToList();

            return result;
        }
    }
}
