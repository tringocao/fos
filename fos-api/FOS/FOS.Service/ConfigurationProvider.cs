using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web;
using FOS.API.Models;
using System.Runtime.Caching;
using FOS.Common;
using FOS.Model.Domain;
using System.Web.Script.Serialization;

namespace FOS.Services
{
    public class ConfigurationModel 
    {
        public Guid SiteId { get; set; }
        public Guid ClientId { get; set; }
        public bool Producion { get; set; }

        public ConfigurationModel() {
            //doc tu web.config 
        }

    }
}
