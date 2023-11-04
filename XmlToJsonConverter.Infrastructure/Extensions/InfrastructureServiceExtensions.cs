using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using XmlToJsonConverter.Domain.Interfaces;
using XmlToJsonConverter.Infrastructure.FileConverters;
using XmlToJsonConverter.Infrastructure.Repositories;

namespace XmlToJsonConverter.Infrastructure.Extensions;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IFileConverter, FileConverter>();
        services.Configure<FileSettings>(configuration.GetSection(nameof(FileSettings)));
        services.PostConfigure<FileSettings>(settings =>
        {
            if (string.IsNullOrWhiteSpace(settings.OutputDirectory))
            {
                throw new ArgumentException("Output directory for file storage must be set in the configuration.");
            }
        });

        services.AddScoped<IFileRepository, FileRepository>(serviceProvider =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<FileSettings>>().Value;
            return new FileRepository(options.OutputDirectory);
        });

        return services;
    }
}
