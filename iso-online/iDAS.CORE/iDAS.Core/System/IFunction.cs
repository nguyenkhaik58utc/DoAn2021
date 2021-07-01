using System;

namespace iDAS.Core
{
    /// <summary>
    /// Interface Function System
    /// </summary>
    public interface IFunction : IModule
    {
        string ModuleCode { get; set; }

        string ParentCode { get; set; }

        bool IsGroup { get; set; }

        string Url { get; set; }

        Type Type { get; set; }
    }
}
