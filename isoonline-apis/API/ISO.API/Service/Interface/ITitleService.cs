using ISO.API.Models;
using System.Collections.Generic;

namespace ISO.API.Service
{
    public interface ITitleService
    {
        TitleListRespDTO GetData(int pageIndex, int pageSize);
        TitleDTO GetByID(int ID);
        int Insert(TitleDTO title);
        bool Update(TitleDTO title);
        bool Delete(int ID);
        List<PositionCboDTO> GetComboPosition();
        List<TitleDTO> GetNotAsDepartment(int departmentID);
    }
}
