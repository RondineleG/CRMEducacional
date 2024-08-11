namespace CRMEducacional.API.Ioc;

public static class NativeInjectorConfig
{
    public static void RegisterApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env
    )
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerAndConfigApiVersioning();
        services.AddAndConfigSwagger(env);
        var connection = configuration["DefaultConnection:ConnectionString"];
        services.AddDbContext<EntityFrameworkDataContext>(options => options.UseSqlServer(connection));
        services.RegisterLogServices();
        services.RegisterServicesAndRepositoriesServices();
    }

    public static void UseApplicationServices(this WebApplication app)
    {
        app.UseExceptionHandler("/error");

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCustomSwaggerUI();
        }

        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseSerilogRequestLogging();
        app.MapControllers();
    }
}