using CRMEducacional.API.LogConfigurations;
using CRMEducacional.Core.Exceptions;

var builder = WebApplication.CreateBuilder(args);

var hostEnvironment = builder.Environment;
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
builder
    .Configuration.SetBasePath(hostEnvironment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{hostEnvironment.EnvironmentName}.json",
        optional: true,
        reloadOnChange: true
    )
    .AddEnvironmentVariables();

if (hostEnvironment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);
}
var loggingSection = builder.Configuration.GetSection("Logging");
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
       .AddConfiguration(loggingSection)
       .AddSimpleConsole()
       .AddConsoleFormatter<CsvLogFormatterConfiguration, ConsoleFormatterOptions>();
});

builder.Services.RegisterApplicationServices(builder.Configuration, hostEnvironment);
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();
app.UseApplicationServices();
app.UseCors("AllowAll");

try
{
    Log.Information("Iniciando o WebApi");
    await app.RunAsync();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Erro catastrófico ao iniciar a WebApi.");
    throw new WebApiStartupException("Ocorreu um erro ao iniciar a WebApi.", exception);
}
finally
{
    await Log.CloseAndFlushAsync();
}