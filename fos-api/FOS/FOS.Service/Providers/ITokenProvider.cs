using FOS.API.Models;
using FOS.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.Providers
{
    public interface ITokenProvider
    {
        Token GetTokenByResourceUrl(string resourceUrl);
        void SaveTokenResource(TokenResource tokenResource);
        TokenResource GetTokenResourceFromRequest();
    }
}
