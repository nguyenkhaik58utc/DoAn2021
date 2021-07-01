using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace iDAS.Services
{
    public class BaseService
    {
        public UserPrincipal User
        {
            get
            {
                return HttpContext.Current.User as UserPrincipal;
            }
        }
    }
}
