using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace XmlToJsonConverter.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services)
        => services.AddMediator();

    private static IServiceCollection AddMediator(this IServiceCollection services)
        => services
        .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
}
