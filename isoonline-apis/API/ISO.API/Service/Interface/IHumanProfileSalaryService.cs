using ISO.API.Models.HumanProfileSalary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IHumanProfileSalaryService
    {
        /// <summary>
        /// Lấy toàn bộ hồ sơ lương theo nhân viên ( không phân trang )
        /// </summary>
        /// <returns></returns>
        List<HumanProfileSalaryDTO> GetAllByEmployeeID(int employeeID);
    }
}
