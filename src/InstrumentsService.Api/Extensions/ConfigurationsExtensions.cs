using InstrumentsService.Domain.Configurations;
namespace InstrumentsService.Api.Extensions;

public static class ConfigurationsExtensions
{
    public static IServiceCollection AddServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DataProviderConfiguration>(options =>
        {
            configuration.GetSection("DataProviderConfiguration").Bind(options);
        });

        return services;
    }
}
