using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using System.Configuration;
using System.Security;
using Microsoft.SharePoint.Client;
using FOS.CoreService.Constants;
using Newtonsoft.Json;
using FOS.Services;
using FOS.Services.SPUserService;
using Microsoft.SharePoint.Client.Utilities;
using FOS.Services.Providers;
using FOS.Model.Mapping;
using Unity.Lifetime;
using FOS.API;
using FOS.Services.SummaryService;
using FOS.Services.SPListService;
using FOS.CoreService.EventServices;

namespace FOS.CoreService.UnityConfig
{
    public class RegisterUnity
    {
        public static void Register(UnityContainer container)
        {
            //container.RegisterType<FosCoreService>();
            //container.RegisterType<Repositories.FosContext, Repositories.FosContext>(
            //   new PerResolveLifetimeManager());
            container.RegisterType<Repositories.Mapping.IOrderMapper, Repositories.Mapping.OrderMapper>();
            container.RegisterType<Repositories.Repositories.IOrderRepository, Repositories.Repositories.OrderRepository>();
            //container.RegisterType<Repositories.Infrastructor.IDbFactory, Repositories.Infrastructor.DbFactory>();
            //container.RegisterType<Repositories.Repositories.IFOSFoodServiceAPIsRepository, Repositories.Repositories.FOSFoodServiceAPIsRepository>();
            //container.RegisterType<Repositories.Repositories.IReportFileRepository, Repositories.Repositories.ReportFileRepository>();
            //container.RegisterType<Repositories.Repositories.IFOSFavoriteRestaurantRepository, Repositories.Repositories.FOSFavoriteRestaurantRepository>();
            container.RegisterType<Services.ExternalServices.IExternalServiceFactory, Services.ExternalServices.ExternalServiceFactory>();
            container.RegisterType<Services.IFOSFoodServiceAPIsService, Services.FOSFoodServiceAPIsService>();
            container.RegisterType<Services.DeliveryServices.IDeliveryService, Services.DeliveryServices.DeliveryService>();
            container.RegisterType<Services.ProvinceServices.IProvinceService, Services.ProvinceServices.ProvinceService>();
            container.RegisterType<Services.RestaurantServices.IRestaurantService, Services.RestaurantServices.RestaurantService>();
            container.RegisterType<Services.FavoriteService.IFavoriteService, Services.FavoriteService.FavoriteService>();
            container.RegisterType<Services.EventServices.IEventService, Services.EventServices.EventService>();
            container.RegisterType<Services.FoodServices.IFoodService, Services.FoodServices.FoodService>();
            container.RegisterType<Services.SendEmailServices.ISendEmailService, Services.SendEmailServices.SendEmailService>();
            container.RegisterType<Services.OrderServices.IOrderService, Services.OrderServices.OrderService>();
            container.RegisterType<Services.IOAuthService, Services.OAuthService>();
            container.RegisterType<Model.Mapping.IOrderDtoMapper, Model.Mapping.OrderDtoMapper>();
            //container.RegisterType<ICustomAuthentication, CustomAuthentication>();
            container.RegisterType<Model.Mapping.IAPIsDtoMapper, Model.Mapping.APIsDtoMapper>();
            container.RegisterType<Model.Mapping.ICategoryDtoMapper, Model.Mapping.CategoryDtoMapper>();
            container.RegisterType<Model.Mapping.ICategoryGroupDtoMapper, Model.Mapping.CategoryGroupDtoMapper>();
            container.RegisterType<Model.Mapping.IDeliveryInfosDtoMapper, Model.Mapping.DeliveryInfosDtoMapper>();
            container.RegisterType<Model.Mapping.IFavoriteRestaurantDtoMapper, Model.Mapping.FavoriteRestaurantDtoMapper>();
            container.RegisterType<Model.Mapping.IFoodCategoryDtoMapper, Model.Mapping.FoodCategoryDtoMapper>();
            container.RegisterType<Model.Mapping.IFoodDtoMapper, Model.Mapping.FoodDtoMapper>();
            container.RegisterType<Model.Mapping.IProvinceDtoMapper, Model.Mapping.ProvinceDtoMapper>();
            container.RegisterType<Model.Mapping.IRestaurantDetailDtoMapper, Model.Mapping.RestaurantDetailDtoMapper>();
            container.RegisterType<Model.Mapping.IRestaurantDtoMapper, Model.Mapping.RestaurantDtoMapper>();
            container.RegisterType<Model.Mapping.IEventDtoMapper, Model.Mapping.EventDtoMapper>();
            container.RegisterType<Model.Mapping.IUserDtoMapper, Model.Mapping.UserDtoMapper>();
            container.RegisterType<IGraphApiProvider, GraphApiProvider>();
            container.RegisterType<ISharepointContextProvider, SharepointContextProvider>();
            container.RegisterType<ITokenProvider, TokenProvider>();
            container.RegisterType<ISPListService, SPListService>();
            container.RegisterType<ISPUserService, SPUserService>();
            container.RegisterType<ISummaryService, SummaryService>();
        }
    }
}
