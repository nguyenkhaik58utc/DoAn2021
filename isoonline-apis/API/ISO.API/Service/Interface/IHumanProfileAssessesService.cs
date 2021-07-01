using ISO.API.Models.HumanProfileAssesses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IHumanProfileAssessesService
    {
        /// <summary>
        /// Lấy toàn bộ hồ sơ đánh giá theo nhân viên ( không phân trang )
        /// </summary>
        /// <returns></returns>
        List<HumanProfileAssessesDTO> GetAllByEmployeeID(int employeeID);
    }
}
