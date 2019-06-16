namespace CustomerApp
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.Linq;
    using System.Web.Http.Dependencies;

    public class MefDependencyResolver : IDependencyResolver
    {
        private readonly CompositionContainer _container;
        private readonly List<Lazy<object, object>> _thisScopesExports = new List<Lazy<object, object>>();
        private readonly object _exportLock = new object();

        public MefDependencyResolver(CompositionContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            if(serviceType is null) throw new ArgumentNullException(nameof(serviceType));

            var export = _container
                .GetExports(serviceType, null, null)
                .FirstOrDefault();

            if (export is null) return null;

            lock (_exportLock)
            {
                _thisScopesExports.Add(export);
            }

            return export.Value;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType is null) throw new ArgumentNullException(nameof(serviceType));

            var exports = _container
                .GetExports(serviceType, null, null)
                .ToArray();

            if (!exports.Any()) return Enumerable.Empty<object>();

            lock (_exportLock)
            {
                _thisScopesExports.AddRange(exports);
            }

            return exports.Select(export => export.Value);
        }

        public IDependencyScope BeginScope()
        {
            return new MefDependencyResolver(_container);
        }

        public void Dispose()
        {
            lock (_exportLock)
            {
                _container.ReleaseExports(_thisScopesExports);
                _thisScopesExports.Clear();
            }
        }
    }
}