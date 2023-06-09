using LoggingComponents;
using LoggingComponents.Serilog;
using LoggingComponents.WebApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services
    .AddHttpClient()
    .AddLogging(l => l.AddSerilog(dispose: true))
    .AddCorrelationIdHttpFilter();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    // Enrich all logs with the name of the service
    .Enrich.WithProperty("ServiceName", Environment.GetEnvironmentVariable("ServiceName") ?? "Unknown service")
    // Add correlation id enricher to static serilog logger
    .Enrich.With<CorrelationIdEnricher>()
    .WriteTo.Seq(serverUrl: "http://seq:5341")
    .CreateLogger();

// Add correlation id to all outgoing requests using HttpClient
builder.Services.AddCorrelationIdHttpFilter();

var app = builder.Build();

// Fetch correlation id from header in incoming requests
app.UseCorrelationIdPropagation();

app.MapControllers();

app.Run();
