using System;
using System.Collections.Generic;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Infrastructure.Resolution;

namespace Ninject.Extensions.Infrastructure
{
    public sealed class IocContainer
    {
        private readonly object lockObject = new object();

        public IKernel Resolver { get; private set; }

        public Func<string, TInterface> ResolveUsingKey<TInterface>() where TInterface : class
        {
            return key => ServiceLocatorProvider.Resolver.GetInstance<TInterface>(key);
        }

        public IocContainer WireDependenciesInAssemblyContaining<T>()
        {
            ScanAssembliesUsing(x => x.FromAssemblyContaining(typeof(T)));
            return this;
        }

        public IocContainer WireDependenciesInAssemblyMatching(string assemblyName)
        {
            ScanAssembliesUsing(x => x.FromAssembliesMatching(assemblyName));
            return this;
        }

        public IocContainer WireDependenciesInAssemblies(params string[] assemblies)
        {
            return WireDependenciesInAssemblies(assemblies as IEnumerable<string>);
        }

        public IocContainer WireDependenciesInAssemblies(IEnumerable<string> assemblies)
        {
            ScanAssembliesUsing(x => x.From(assemblies));
            return this;
        }

        private void ScanAssembliesUsing(Action<AssemblyScanner> assemblyScanningAlgorithm)
        {
            if (DontHaveAResolverYet)
            {
                lock (lockObject)
                {
                    if (DontHaveAResolverYet)
                    {
                        var kernel = new StandardKernel(new NinjectSettings { LoadExtensions = false, InjectNonPublic = true });

                        kernel.Scan(x =>
                        {
                            assemblyScanningAlgorithm(x);
                            x.AutoLoadModules();
                        });

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