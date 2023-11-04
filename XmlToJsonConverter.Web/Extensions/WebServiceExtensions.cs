using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using XmlToJsonConverter.Domain.Interfaces;
using XmlToJsonConverter.Infrastructure.FileConverters;
using XmlToJsonConverter.Infrastructure.Repositories;

namespace XmlToJsonConverter.Web.Extensions;

public static class WebServiceExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
        => services.AddFluentValidation();

    public static IServiceCollection AddFluentValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}
