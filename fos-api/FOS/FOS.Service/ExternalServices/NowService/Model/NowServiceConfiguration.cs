using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.ExternalServices.NowService
{
    public class NowServiceConfiguration
    {
        public APIDetail GetRestaurantDeliveryInfor { get; set; }
        public APIDetail GetMetadata { get; set; }
        public APIDetail GetDeliveryDishes { get; set; }
        public APIDetail SearchRestaurantsInProvince { get; set; }
        public APIDetail GetDeliveryFromUrl { get; set; }
    }
    public class APIDetail
    {
        public string API { get; set; }
        public string RequestMethod { get; set; }
        public List<AvailableField> AvailableHeaders { get; set; }
        public List<AvailableField> AvailableBodys { get; set; }
        public List<AvailableField> AvailableParams { get; set; }
    }
    public class AvailableField
    {
        public string FieldName { get; set; }
        public string ValueDefault { get; set; }

    }

}
//}

//{   
//    "GetAllRestaurant":[{
//        "API": <API Link>,
//        "RequestMethod": <Post, Get,...>
//        "AvailableHeaders":[{
//            "FieldName":<Value Default>,
//            ...
//        }],
//        "AvailableBodys":[{ --use for post
//            "FieldName":<Value Suggestions>,
//            ...
//        }]
//        "AvailableParams":[{ --use for get
//            "FieldName":<Value Suggestions>,
//            ...
//        }]
//    }],
//    ...
//}