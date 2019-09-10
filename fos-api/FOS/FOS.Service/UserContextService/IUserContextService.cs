using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.UserContextService
{
    public interface IUserContextService
    {
        User GetUserContext();
    }
}
