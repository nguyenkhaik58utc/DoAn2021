using ISO.API.Models.HumanProfileWorkTrial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IHumanProfileWorkTrialService
    {
        /// <summary>
        /// Lấy toàn bộ hồ sơ lương theo nhân viên ( không phân trang )
        /// </summary>
        /// <returns></returns>
        List<HumanProfileWorkTrialDTO> GetAllByEmployeeID(int employeeID);
    }
}
