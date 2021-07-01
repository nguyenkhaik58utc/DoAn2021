using ISO.API.Models.Position;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IPositionService
    {
        List<PositionDTO> GetAll();
        int AddPosition(object param);

        int deletePosition(int id);

        int UpdatePosition(object param);
        List<PositionDTO> GetByID(int id);
    }
}
