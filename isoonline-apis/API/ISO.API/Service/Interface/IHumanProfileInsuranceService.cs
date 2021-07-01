using ISO.API.Models.HumanProfileInsurances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IHumanProfileInsuranceService
    {
        /// <summary>
        /// Lấy toàn bộ hồ sơ quan hệ gia đình theo nhân viên ( không phân trang )
        /// </summary>
        /// <returns></returns>
        List<HumanProfileInsurancesDTO> GetAllByEmployeeID(int employeeID);
    }
}
