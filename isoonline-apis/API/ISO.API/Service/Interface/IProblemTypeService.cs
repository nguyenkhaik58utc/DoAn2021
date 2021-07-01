using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IProblemTypeService
    {
        List<ProblemTypeDTO> GetAll();
        int Add(object param);

        int Delete(int id);

        int Update(object param);
        List<ProblemTypeDTO> GetByID(int id);
        List<ProblemTypeDTO> GetListByID(int id);
        int Default(int id);
    }
}
