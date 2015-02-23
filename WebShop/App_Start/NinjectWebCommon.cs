[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebShop.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebShop.App_Start.NinjectWebCommon), "Stop")]

namespace WebShop.App_Start
{
	using System;
	using System.Web;
	using System.Web.Configuration;
	using Microsoft.Web.Infrastructure.DynamicModuleHelper;
	using Ninject;
	using Ninject.Web.Common;
	using WebShop.Services;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
			kernel.Bind<ISettingsService>().To<SettingsService>().InSingletonScope();

			kernel.Bind<ICatalogService>().ToMethod(context =>
			{
				string appDataPath = HttpContext.Current.Server.MapPath("~/App_Data");
				string file = System.Text.RegularExpressions.Regex.Replace(WebConfigurationManager.AppSettings["catalog:path"], @"\|DataDirectory\|", appDataPath, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				return CatalogService.Create(file);
			}).InSingletonScope();
        }        
    }
}
