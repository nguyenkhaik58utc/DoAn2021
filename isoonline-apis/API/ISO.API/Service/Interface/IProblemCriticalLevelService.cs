using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IProblemCriticalLevelService
    {
        List<ProblemCriticalLevelDTO> GetAll();
        int AddProblemCriticalLevel(object param);

        int DeleteProblemCriticalLevel(int id);

        int UpdateProblemCriticalLevel(object param);
        List<ProblemCriticalLevelDTO> GetByID(int id);
        int Default(int id);
    }
}
