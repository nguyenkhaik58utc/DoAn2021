using ISO.API.DataAccess;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public class MenuService : IMenuService
    {
        public List<MenuDTO> GetAll()
        {
            ExecuteQuery exec = new ExecuteQuery();
            List<MenuDTO> menuDTOs = exec.GetAll<MenuDTO>("sp_V3OrgMenu_GetAll", commandType: CommandType.StoredProcedure);
            return menuDTOs;
        }

        public List<MenuDTO> GetBusinessModule(int userID)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { userID = userID };
            List<MenuDTO> menuDTOs = exec.GetByField<MenuDTO>("sp_V3OrgMenu_TreeMenuBusinessModule", param, commandType: CommandType.StoredProcedure);
            return menuDTOs;
        }

        public List<MenuDTO> GetBusinessFunction(int userID, string ModuleCode)
        {
            ExecuteQuery exec = new ExecuteQuery();
            object param = new { userID = userID, ModuleCode = ModuleCode };
            List<MenuDTO> menuDTOs = exec.GetByField<MenuDTO>("sp_V3OrgMenu_TreeMenuBusinessFunction", param, commandType: CommandType.StoredProcedure);
            return menuDTOs;
        }
    }
}
