using System;

using Unity;
using Unity.Lifetime;

namespace FOS.API
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            container.RegisterType<Repositories.FosContext, Repositories.FosContext>(new PerResolveLifetimeManager());
            container.RegisterType<Repositories.Mapping.IOrderMapper, Repositories.Mapping.OrderMapper>();
            container.RegisterType<Repositories.IOrderRepository, Repositories.OrderRepository>();
            container.RegisterType<Repositories.Infrastructor.IDbFactory, Repositories.Infrastructor.DbFactory>();
            container.RegisterType<Repositories.Repositories.IFOSFoodServiceAPIsRepository, Repositories.Repositories.FOSFoodServiceAPIsRepository>();
            //container.RegisterType<Repositories.Repositories.IFOSHostLinkRepository, Repositories.Repositories.FOSHostLinkRepository>();

            container.RegisterType<Services.FoodServices.IExternalServiceFactory, Services.FoodServices.ExternalServiceFactory>();
            container.RegisterType<Services.IFOSFoodServiceAPIsService, Services.FOSFoodServiceAPIsService>();
            container.RegisterType<Services.DeliveryServices.IDeliveryService, Services.DeliveryServices.DeliveryService>();
            container.RegisterType<Services.ProvinceServices.IProvinceService, Services.ProvinceServices.ProvinceService>();
            container.RegisterType<Services.RestaurantServices.IRestaurantService, Services.RestaurantServices.RestaurantService>();

            //container.RegisterType<Services.ICrawlLinksService, Services.CrawlLinksService>();
            //container.RegisterType<Repositories.APIExternalServiceEntities, Repositories.APIExternalServiceEntities>(new PerResolveLifetimeManager());
            //container.RegisterType<Services.RequestMethods.IRequestMethod, Services.RequestMethods.GetMethod> ("GetMethod");
            //container.RegisterType<Services.RequestMethods.IRequestMethod, Services.RequestMethods.PostMethod>("PostMethod");
            container.RegisterType<Services.IOrderService, Services.OrderService>();
            container.RegisterType<Services.IOAuthService, Services.OAuthService>();
            container.RegisterType<Model.Mapping.IOrderDtoMapper, Model.Mapping.OrderDtoMapper>();

            container.RegisterType<ICustomAuthentication, CustomAuthentication>();
            container.RegisterType<Model.Mapping.IAPIsDtoMapper, Model.Mapping.APIsDtoMapper>();

            container.AddExtension(new Diagnostic());
        }
    }
}