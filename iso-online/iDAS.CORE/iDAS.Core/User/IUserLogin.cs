namespace iDAS.Core
{
    /// <summary>
    /// Interface User Login
    /// </summary>
    public interface IUserLogin
    {
        string Account { get; set; }

        string Password { get; set; }

        string Code { get; set; }

        bool Remember { get; set; }

        string Languague { get; set; }
    }
}
