using InstrumentsService.Api.Errors;
using InstrumentsService.Application.Helpers;
using InstrumentsService.Application.Interfaces;
using InstrumentsService.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InstrumentsService.Api.Extensions;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFinancialInstrumentService, FinancialInstrumentService>();
        services.AddScoped<IDataProvider, TiingoDataProvider>();
        services.AddScoped<IDataProvider, BinanceDataProvider>();
        services.AddScoped<DataProviderResolver>();
        services.AddScoped<TiingoDataProvider>(); 
        services.AddScoped<BinanceDataProvider>();

        services.Configure<ApiBehaviorOptions>(options =>
          options.InvalidModelStateResponseFactory = ActionContext =>
          {
              var error = ActionContext.ModelState
                          .Where(e => e.Value!.Errors.Count > 0)
                          .SelectMany(e => e.Value!.Errors)
                          .Select(e => e.ErrorMessage).ToArray();
              var errorresponce = new ApiValidationErrorResponse
              {
                  Errors = error
              };
              return new BadRequestObjectResult(error);
          }
        );
        return services;
    }
}
