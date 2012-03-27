using System.Collections.Generic;

namespace Ninject.Extensions.Infrastructure.Resolution
{
    /// <summary>
    /// abstract dependancy resolver contract
    /// </summary>
    public interface IDependancyResolver
    {
        T GetInstance<T>();
        T GetInstance<T>(string bindingKey);
        IEnumerable<T> GetInstances<T>();
    }
}