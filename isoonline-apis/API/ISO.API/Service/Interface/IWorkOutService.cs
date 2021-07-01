using ISO.API.Models.WorkOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service.Interface
{
    public interface IWorkOutService
    {
        List<WorkOutViewModel> GetByMonth(out int totalRows, GetWorkOutByMonthRequest request);

        WorkOutDTO GetById(int id);

        int Insert(InsertWorkOutRequest request);

        int Update(UpdateWorkOutRequest request);

        int CheckExist(CheckExistWorkOutRequest request);

        int Delete(int id);
    }
}