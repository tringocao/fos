﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain
{
    public class AuthClientRespond
    {
        public bool redirect { get; set; }
        public string redirectUrl { get; set; }
    }
}
