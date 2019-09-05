using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class State
    {
        public string redirectUri { get; set; }

        public State(string _redirectUri)
        {
            redirectUri = _redirectUri;
        }
    }
}
