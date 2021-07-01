using System;

namespace iDAS.Core
{
    /// <summary>
    /// Interface Config System
    /// </summary>
    public interface IConfig
    {
        Type UserService { get; }
        string Culture { get; }
        string ConfigUrl { get; }
        string ConfigFilePath { get; }
        string SystemCode { get; }
        string PathUpload { get; }
    }
}
