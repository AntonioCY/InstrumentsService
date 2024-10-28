namespace InstrumentsService.Api.Extensions;

public static class SwaggerServicesExtensions
{
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen();
        // extend it when Authentication/Authorization would be added 

        return services;
    }

    public static IApplicationBuilder UseSwaggerGen(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
        return app;
    }
}
