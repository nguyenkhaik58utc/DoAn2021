using ISO.API.Models.HumanProfileAttachment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IHumanProfileAttachmentService
    {
        /// <summary>
        /// Lấy toàn bộ hồ sơ đính kèm theo nhân viên ( không phân trang )
        /// </summary>
        /// <returns></returns>
        List<HumanProfileAttachmentDTO> GetAllByEmployeeID(int employeeID);
    }
}
