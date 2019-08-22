using FOS.Model.Domain;
using FOS.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FOS.Services.FoodServices
{
    public class ExternalServiceFactory: IExternalServiceFactory
    {
        private IFOSFoodServiceAPIsService _foodServiceAPIsService;
        //private IHostLinkService _hostLinkSer;
        //private IEnumerable<IRequestMethod> _requestMethod { get; }

        //private IRequestMethod _requestMethod;s
        public ExternalServiceFactory(IFOSFoodServiceAPIsService foodServiceAPIsService)
        {
            this._foodServiceAPIsService = foodServiceAPIsService;
        }
        public string Service(int id)
        {
            APIsDTO apis = _foodServiceAPIsService.GetById(id);
            IFoodService test = GetFoodService(apis);
            var re1 = test.GetRestaurantsAsync();
            Restaurant a = new Restaurant() { delivery_id = 607, restaurant_id = 595 };
            var re2 = test.GetFoods(a);
            return "";

        }
        //private Task<string> RunAPI(APIs api)
        //{
        //    //var method = GetMethod(api);
        //    return;//method.GetResultAsync();
        //}
        public IFoodService GetFoodService(APIsDTO api)
        {
            switch (api.TypeService)
            {
                case ServiceKind.Now:
                    {                      
                        return new NowService.NowService(api);
                    }
                case ServiceKind.GrabFood:
                    {
                        return new GrabFoodService.GrabFoodService();
                    }
                default:
                    throw new Exception();
            }
        }

        //public Task<string> API(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
