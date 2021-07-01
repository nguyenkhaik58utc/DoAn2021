using ISO.API.Models;

namespace ISO.API.Service
{
    public interface IEmployeeOTService
    {
        EmployeeOTListResp GetData(int pageIndex, int pageSize);
        EmployeeOT GetByID(int ID);
        int Insert(EmployeeOT title);
        bool Update(EmployeeOT title);
        bool Delete(int ID);
    }
}
