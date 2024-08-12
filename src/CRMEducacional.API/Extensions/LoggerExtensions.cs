namespace CRMEducacional.API.Extensions;

public static class LoggerExtensions
{
    public static void CustomFormatLog(
          this ILogger logger,
          LogEventLevel level,
          string controllerName,
          string actionName,
          string messageTemplate
      )
    {
        var formattedMessageTemplate = $"{controllerName} => {actionName} => {messageTemplate}";

        switch (level)
        {
            case LogEventLevel.Verbose:
            logger.Verbose(formattedMessageTemplate);
            break;

            case LogEventLevel.Debug:
            logger.Debug(formattedMessageTemplate);
            break;

            case LogEventLevel.Information:
            logger.Information(formattedMessageTemplate);
            break;

            case LogEventLevel.Warning:
            logger.Warning(formattedMessageTemplate);
            break;

            case LogEventLevel.Error:
            logger.Error(formattedMessageTemplate);
            Log.CloseAndFlush();
            break;

            case LogEventLevel.Fatal:
            logger.Fatal(formattedMessageTemplate);
            Log.CloseAndFlush();
            break;

            default:
            logger.Information(formattedMessageTemplate);
            break;
        }
    }

    public static void RegisterLogServices(this IServiceCollection services)
    {
        services.AddSingleton(Log.Logger);
    }
}