using Microsoft.Extensions.DependencyInjection;
using Solid.UnitOfWork.Base;
using Solid.UnitOfWork.Interfaces;
using System.Linq.Expressions;

namespace Solid.UnitOfWork.Extensions;

public static class IServiceCollectionExtension
{
    private static readonly Dictionary<ServiceLifetime, Expression<Func<IServiceCollection, IServiceCollection>>> LifetimeDict = new Dictionary<ServiceLifetime, Expression<Func<IServiceCollection, IServiceCollection>>>()
    {
        {ServiceLifetime.Transient,service => service.AddTransient(typeof(IUnitOfWork<>), typeof(BaseUnitOfWork<>)) },
        {ServiceLifetime.Scoped,service => service.AddScoped(typeof(IUnitOfWork<>), typeof(BaseUnitOfWork<>)) },
        {ServiceLifetime.Singleton,service => service.AddSingleton(typeof(IUnitOfWork<>), typeof(BaseUnitOfWork<>)) },
    };

    public static IServiceCollection AddRepositories(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var func = LifetimeDict[serviceLifetime];

        func.Compile().Invoke(services);

        return services;
    }
}