namespace iDAS.Core
{
    /// <summary>
    /// Interface Action System
    /// </summary>
    public interface IAction : ISystem
    {
        string ModuleCode { get; set; }

        string FunctionCode { get; set; }
    }
}
