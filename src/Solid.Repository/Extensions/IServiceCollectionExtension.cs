using Microsoft.Extensions.DependencyInjection;
using Solid.Repository.Base;
using Solid.Repository.Interfaces;
using System.Linq.Expressions;

namespace Solid.Repository.Extensions;

public static class IServiceCollectionExtension
{
    private static readonly Dictionary<ServiceLifetime, Expression<Func<IServiceCollection, IServiceCollection>>> LifetimeDict = new Dictionary<ServiceLifetime, Expression<Func<IServiceCollection, IServiceCollection>>>()
    {
        {ServiceLifetime.Transient,service => service.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>)) },
        {ServiceLifetime.Scoped,service => service.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>)) },
        {ServiceLifetime.Singleton,service => service.AddSingleton(typeof(IRepository<>), typeof(BaseRepository<>)) },
    };

    public static IServiceCollection AddRepositories(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        var func = LifetimeDict[serviceLifetime];

        func.Compile().Invoke(services);

        return services;
    }
}