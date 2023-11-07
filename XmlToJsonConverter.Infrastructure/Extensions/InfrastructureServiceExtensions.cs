using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO.Abstractions;
using XmlToJsonConverter.Domain.Interfaces;
using XmlToJsonConverter.Infrastructure.Converters;
using XmlToJsonConverter.Infrastructure.Repositories;

namespace XmlToJsonConverter.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
        => services.AddFileConverter()
            .AddFileSystem()
            .AddFileSettings(configuration)
            .AddFileRepository();

    public static IServiceCollection AddFileSystem(this IServiceCollection services)
    {
        services.AddSingleton<IFileSystem, FileSystem>();
        return services;
    }

    public static IServiceCollection AddFileConverter(this IServiceCollection services)
        => services.AddTransient<IXmlToJsonConverter, XmlToJsonConverterService>();

    public static IServiceCollection AddFileRepository(
        this IServiceCollection services)
    {
        services.AddScoped<IFileRepository, FileRepository>();

        return services;
    }

    public static IServiceCollection AddFileSettings(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FileSettings>(configuration.GetSection(nameof(FileSettings)));
        services.PostConfigure<FileSettings>(settings =>
        {
            if (string.IsNullOrWhiteSpace(settings.OutputDirectory))
            {
                throw new ArgumentException(
                    "Output directory for file storage must be set in the configuration.");
            }
        });
        return services;
    }
}
