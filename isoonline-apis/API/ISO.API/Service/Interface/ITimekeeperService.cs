using ISO.API.Models;

namespace ISO.API.Service
{
    public interface ITimekeeperService
    {
        TimekeeperListRespDTO GetData();
        TimekeeperDTO GetByID(int ID);
        int Insert(TimekeeperDTO timekeeper);
        bool Update(TimekeeperDTO timekeeper);
        bool Delete(int ID);
        bool InputTimekeeping(int TimkeeperID, string IP, int Post, out string Error);
    }
}
