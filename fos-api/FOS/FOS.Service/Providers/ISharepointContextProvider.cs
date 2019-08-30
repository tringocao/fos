using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.Providers
{
    public interface ISharepointContextProvider
    {
        ClientContext GetSharepointContextFromUrl(string siteUrl);
    }
}
