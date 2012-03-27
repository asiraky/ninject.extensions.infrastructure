using System;

namespace Ninject.Extensions.Infrastructure.Resolution
{
    /// <summary>
    /// holds a registered <see cref="IDependancyResolver"/> to allow servicelocation 
    /// throughout a container managed application
    /// </summary>
    public static class ServiceLocatorProvider
    {
        public static void RegisterProvider(IDependancyResolver dependancyResolver)
        {
            resolver = dependancyResolver;
        }

        private static IDependancyResolver resolver;
        public static IDependancyResolver Resolver
        {
            get
            {
                if (resolver == null)
                {
                    throw new InvalidOperationException("the ServiceLocatorProvider hasnt been registered yet.");
                }

                return resolver;
            }
        }
    }
}