using FOS.Model.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Model.Domain.NowModel
{
    public class EmailTemplate
    {
        public List<User> UsersEmail { get; set; }
        public User HostUserEmail { get; set; }
        [JsonProperty("html")]
        public string Html { get; set; }
        [JsonProperty("subject")]
        public string Subject { get; set; }
        public string EventTitle { get; set; }
        public string EventId { get; set; }
        public string EventRestaurant { get; set; }
        public string EventRestaurantId { get; set; }
        public string EventDeliveryId { get; set; }
        public string MakeOrder { get; set; }
        public string FeedBack { get; set; }
        public string NotParticipant { get; set; }
    }
}
