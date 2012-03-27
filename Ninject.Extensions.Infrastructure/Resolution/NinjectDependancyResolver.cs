using System.Collections.Generic;

namespace Ninject.Extensions.Infrastructure.Resolution
{
    /// <summary>
    /// holds a reference to a ninject kernel in order to implement <see cref="IDependancyResolver"/>
    /// </summary>
    public class NinjectDependancyResolver : IDependancyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependancyResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        #region Implementation of IDependancyResolver

        public T GetInstance<T>()
        {
            return kernel.Get<T>();
        }

        public T GetInstance<T>(string bindingKey)
        {
            return kernel.Get<T>(bindingKey);
        }

        public IEnumerable<T> GetInstances<T>()
        {
            return kernel.GetAll<T>();
        }

        #endregion
    }
}
