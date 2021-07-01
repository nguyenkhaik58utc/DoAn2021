using ISO.API.DataAccess;
using ISO.API.Models.Position;
using ISO.API.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Implement
{
    public class PositionService : IPositionService
    {
        public int AddPosition(object param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int problemCriticalLevelDTO = exec.ExcuteScalar("sp_Position_Insert", param);
            return problemCriticalLevelDTO;
        }

        public int deletePosition(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int positionDTO = exec.ExcuteScalar("sp_Position_Delete", new { ID = id });
            return positionDTO;
        }

        public List<PositionDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<PositionDTO> positionDTO = exec.GetAll<PositionDTO>("sp_Position_GetAll", commandType: CommandType.StoredProcedure);
            return positionDTO;
        }

        public List<PositionDTO> GetByID(int id)
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<PositionDTO> positionDTO = exec.GetByField<PositionDTO>("sp_Position_GetByID", new { ID = id }, commandType: CommandType.StoredProcedure);
            return positionDTO;
        }

        public int UpdatePosition(object param)
        {
            ExecuteQuery exec = new ExecuteQuery();
            int positionDTO = exec.ExcuteScalar("sp_Position_Update", param);
            return positionDTO;
        }
    }
}
