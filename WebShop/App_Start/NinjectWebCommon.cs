[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebShop.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebShop.App_Start.NinjectWebCommon), "Stop")]

namespace WebShop.App_Start
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Web;
	using System.Web.Configuration;
	using System.Web.Http;
	using System.Web.Http.Dependencies;
	using Microsoft.Web.Infrastructure.DynamicModuleHelper;
	using Microsoft.AspNet.Identity.Owin;
	using Ninject;
	using Ninject.Web.Common;
	using Ninject.Syntax;
	using Ninject.Parameters;
	using Ninject.Activation;
	using WebShop.Models;
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
				GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);

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
			kernel.Bind<ApplicationUserManager>().ToMethod(context =>
				{
					return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
				}).InSingletonScope();

			kernel.Bind<ISettingsService>().To<SettingsService>().InSingletonScope();

			kernel.Bind<IFinanceService>().To<FinanceService>().InSingletonScope();

			kernel.Bind<ICatalogService>().ToMethod(context =>
			{
				string appDataPath = HttpContext.Current.Server.MapPath("~/App_Data");
				string file = System.Text.RegularExpressions.Regex.Replace(WebConfigurationManager.AppSettings["catalog:path"], @"\|DataDirectory\|", appDataPath, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

				return CatalogService.Create(file);
			}).InSingletonScope();

			kernel.Bind<IShoppingCartService>().ToMethod(context =>
			{
				ApplicationDbContext dbContext = HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
				return ShoppingCartService.Create(dbContext);
			});

			kernel.Bind<IErpService>().To<ErpService>();
        }        
    }

	public class NinjectScope : IDependencyScope
	{
		protected IResolutionRoot resolutionRoot;

		public NinjectScope(IResolutionRoot kernel)
		{
			resolutionRoot = kernel;
		}

		public object GetService(Type serviceType)
		{
			IRequest request = resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
			return resolutionRoot.Resolve(request).SingleOrDefault();
		}

		public IEnumerable<object> GetServices(Type serviceType)
		{
			IRequest request = resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
			return resolutionRoot.Resolve(request).ToList();
		}

		public void Dispose()
		{
			IDisposable disposable = (IDisposable)resolutionRoot;
			if (disposable != null) disposable.Dispose();
			resolutionRoot = null;
		}
	}

	public class NinjectResolver : NinjectScope, IDependencyResolver
	{
		private IKernel _kernel;
		public NinjectResolver(IKernel kernel)
			: base(kernel)
		{
			_kernel = kernel;
		}
		public IDependencyScope BeginScope()
		{
			return new NinjectScope(_kernel.BeginBlock());
		}
	}

}
