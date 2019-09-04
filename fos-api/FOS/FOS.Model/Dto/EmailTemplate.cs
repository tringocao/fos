using FOS.Model.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Dto
{
    public class EmailTemplate
    {
        public List<User> UsersEmail { get; set; }
        public User HostUserEmail { get; set; }
        [JsonProperty("html")]
        public string Html { get; set; }
        public string Subject { get; set; }
        public string EventTitle { get; set; }
        public string EventRestaurant { get; set; }
        public int OrderId { get; set; }

    }
}
