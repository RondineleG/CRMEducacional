using CRMEducacional.Core.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);
}

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.RegisterApplicationServices(builder.Configuration, builder.Environment);

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