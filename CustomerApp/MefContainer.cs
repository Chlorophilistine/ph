namespace CustomerApp
{
    using System.ComponentModel.Composition.Hosting;
    using System.Reflection;
    using System.Web.Http;
    using DataAccess;

    public static class MefContainer
    {
        public static void RegisterMefDependencyResolver()
        {
            var container = ConfigureContainer();
            GlobalConfiguration.Configuration.DependencyResolver = new MefDependencyResolver(container);
        }

        internal static CompositionContainer ConfigureContainer(bool debug = false)
        {
            var appCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            var dataCatalog = new AssemblyCatalog(typeof(CustomerContext).Assembly);

            var aggregateCatalog = new AggregateCatalog(appCatalog, dataCatalog);

            return debug
                ? new CompositionContainer(aggregateCatalog, CompositionOptions.DisableSilentRejection)
                : new CompositionContainer(aggregateCatalog);
        }
    }
}