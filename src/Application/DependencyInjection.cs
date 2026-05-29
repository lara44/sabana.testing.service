using System.Reflection;
using ATCMediator;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddATCMediator(Assembly.GetExecutingAssembly());
        return services;
    }
}
