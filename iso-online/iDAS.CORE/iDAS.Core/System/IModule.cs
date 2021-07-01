namespace iDAS.Core
{
    /// <summary>
    /// Interface Module System
    /// </summary>
    public interface IModule : ISystem
    {
        bool IsShow { get; set; }

        int Position { get; set; }

        string Icon { get; set; }
    }
}
