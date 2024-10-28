using InstrumentsService.Api.Errors;
using System.Net;
using System.Text.Json;

namespace InstrumentsService.Api.Middleware;

public class ExceptionMiddle(
    RequestDelegate next,
    ILogger<ExceptionMiddle> logger,
    IHostEnvironment hostEnvironment)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddle> _logger = logger;
    private readonly IHostEnvironment _hostEnvironment = hostEnvironment;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _hostEnvironment.IsDevelopment() ?
                new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace!.ToString())
                : new ApiException((int)HttpStatusCode.InternalServerError);

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await httpContext.Response.WriteAsync(json);
        }
    }
}
