namespace iDAS.Core
{
    /// <summary>
    /// Interface System
    /// </summary>
    public interface ISystem
    {
        string Code { get; set; }

        string Name { get; set; }

        bool IsActive { get; set; }
    }
}
