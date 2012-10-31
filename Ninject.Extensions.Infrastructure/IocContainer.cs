using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Extensions.Infrastructure.Resolution;

namespace Ninject.Extensions.Infrastructure
{
    public sealed class IocContainer
    {
        private readonly object lockObject = new object();

        public IKernel Resolver { get; private set; }

        public IocContainer WireDependenciesInAssemblyMatching(string assemblyName)
        {
            ScanAssembliesUsing(x => x.Load(Assembly.Load(assemblyName)));
            return this;
        }

        public IocContainer WireDependenciesInAssemblies(params string[] assemblies)
        {
            ScanAssembliesUsing(x => x.Load(assemblies.Select(Assembly.Load)));
            return this;
        }

        public IocContainer WireDependenciesInAssemblies(IEnumerable<string> assemblies)
        {
            ScanAssembliesUsing(x => x.Load(assemblies.Select(Assembly.Load)));
            return this;
        }

        private void ScanAssembliesUsing(Action<StandardKernel> applytoKernel)
        {
            if (DontHaveAResolverYet)
            {
                lock (lockObject)
                {
                    if (DontHaveAResolverYet)
                    {
                        var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false, InjectNonPublic = true });

                        applytoKernel(kernel);

                        SetKernel(kernel);
                    }
                }
            }
        }

        private void SetKernel(IKernel kernel)
        {
            var dependancyResolver = new NinjectDependancyResolver(kernel);
            ServiceLocatorProvider.RegisterProvider(dependancyResolver);
            Resolver = kernel;
        }

        private bool DontHaveAResolverYet
        {
            get { return Resolver == null; }
        }
    }
}