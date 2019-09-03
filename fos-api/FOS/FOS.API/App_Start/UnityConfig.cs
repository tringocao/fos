using FOS.Services.Providers;
using FOS.Services.SPListService;
using FOS.Services.SPUserService;
using System;

using Unity;
using Unity.Interception;
using Unity.Interception.ContainerIntegration;
using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;
using Unity.Lifetime;
using Unity.log4net;

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
              var container = new UnityContainer()
              .AddNewExtension<Log4NetExtension>()
              .AddNewExtension<Interception>();

              RegisterTypes(container);
  
              return container;
          });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

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
            container.RegisterType<Repositories.FosContext, Repositories.FosContext>(
                new PerResolveLifetimeManager());
            container.RegisterType<Repositories.Mapping.IOrderMapper, Repositories.Mapping.OrderMapper>();
            container.RegisterType<Repositories.IOrderRepository, Repositories.OrderRepository>();
            container.RegisterType<Repositories.Infrastructor.IDbFactory, Repositories.Infrastructor.DbFactory>();
            container.RegisterType<Repositories.Repositories.IFOSFoodServiceAPIsRepository, Repositories.Repositories.FOSFoodServiceAPIsRepository>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<Repositories.Repositories.IFOSFavoriteRestaurantRepository, Repositories.Repositories.FOSFavoriteRestaurantRepository>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            //container.RegisterType<Repositories.Repositories.IFOSHostLinkRepository, Repositories.Repositories.FOSHostLinkRepository>();

            container.RegisterType<Services.ExternalServices.IExternalServiceFactory, Services.ExternalServices.ExternalServiceFactory>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<Services.IFOSFoodServiceAPIsService, Services.FOSFoodServiceAPIsService>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<Services.DeliveryServices.IDeliveryService, Services.DeliveryServices.DeliveryService>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<Services.ProvinceServices.IProvinceService, Services.ProvinceServices.ProvinceService>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<Services.RestaurantServices.IRestaurantService, Services.RestaurantServices.RestaurantService>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<Services.FavoriteService.IFavoriteService, Services.FavoriteService.FavoriteService>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());

            //container.RegisterType<Services.ICrawlLinksService, Services.CrawlLinksService>();
            //container.RegisterType<Repositories.APIExternalServiceEntities, Repositories.APIExternalServiceEntities>(new PerResolveLifetimeManager());
            //container.RegisterType<Services.RequestMethods.IRequestMethod, Services.RequestMethods.GetMethod> ("GetMethod");
            //container.RegisterType<Services.RequestMethods.IRequestMethod, Services.RequestMethods.PostMethod>("PostMethod");
            container.RegisterType<Services.IOrderService, Services.OrderService>();
            container.RegisterType<Services.IOAuthService, Services.OAuthService>(
                new HierarchicalLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<Model.Mapping.IOrderDtoMapper, Model.Mapping.OrderDtoMapper>();

            container.RegisterType<ICustomAuthentication, CustomAuthentication>();
            container.RegisterType<Model.Mapping.IAPIsDtoMapper, Model.Mapping.APIsDtoMapper>();

            container.RegisterType<IGraphApiProvider, GraphApiProvider>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<ISharepointContextProvider, SharepointContextProvider>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<ITokenProvider, TokenProvider>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<ISPListService, SPListService>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());
            container.RegisterType<ISPUserService, SPUserService>(
                new TransientLifetimeManager(),
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptor>());

            container.AddExtension(new Diagnostic());
        }
    }
}