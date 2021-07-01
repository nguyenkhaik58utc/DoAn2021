using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Service
{
    public interface IMenuService
    {
        List<MenuDTO> GetAll();

        List<MenuDTO> GetBusinessModule(int userID);
        List<MenuDTO> GetBusinessFunction(int userID, string ModuleCode);
    }
}
