using System.ComponentModel.DataAnnotations;

namespace iDAS.Core
{
    /// <summary>
    /// Database Class
    /// </summary>
    public class Database
    {
        public string DataSource { get; set; }

        public string DataName { get; set; }

        public string UserId { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Code Invalid!")]
        public string Code { get; set; }

        /// <summary>
        /// Copy object database
        /// </summary>
        public Database Clone()
        {
            return (Database)this.MemberwiseClone();
        }
    }
}
