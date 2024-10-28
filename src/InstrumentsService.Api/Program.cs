using Asp.Versioning;
using InstrumentsService.Api.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using InstrumentsService.Application.Handlers;

var builder = WebApplication.CreateBuilder(args);

var allowSpecificOrigin = "AllowSpecificOrigin";
var cUrl = builder.Configuration.GetSection("CorsBaseUrl").Get<string[]>() ?? ["http://localhost"];

builder.Services.AddCors(options =>
{
    options.AddPolicy(allowSpecificOrigin,
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins(cUrl)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Host.UseSerilog((context, provider, loggerConfiguration) =>
{
    loggerConfiguration
        .Enrich.FromLogContext()
        .Enrich.WithCorrelationIdHeader()
        .Enrich.WithExceptionDetails();

    loggerConfiguration.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);

    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(provider);

    var telemetryConfiguration = provider.GetService<IOptions<TelemetryConfiguration>>();

    if (!string.IsNullOrEmpty(telemetryConfiguration?.Value.ConnectionString))
    {
        loggerConfiguration
            .WriteTo
            .ApplicationInsights(telemetryConfiguration.Value, TelemetryConverter.Traces);
    }
});

builder.Services
    .AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddControllers();
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerDocumentation()
    .AddApplicationServices()
    .AddHttpClient()
    .AddLogging()
    .AddServiceConfiguration(builder.Configuration)
    .AddHealthChecks();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.UseHealthChecks("/api/health");
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

webSocketOptions.AllowedOrigins
    .ToList()
    .AddRange(cUrl);

app.UseWebSockets(webSocketOptions);
app.UseRouting();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await WebSocketHandler.HandleWebSocketAsync(webSocket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});

_ = Task.Run(async () =>
{
    await WebSocketHandler.SubscribeToExternalWebSocket(app.Configuration);
});

app.Run();
