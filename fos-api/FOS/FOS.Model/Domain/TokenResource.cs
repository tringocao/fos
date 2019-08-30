using FOS.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class TokenResource
    {
        public List<Token> tokens { get; set; }
        public string RefreshToken { get; set; }
    }
}
