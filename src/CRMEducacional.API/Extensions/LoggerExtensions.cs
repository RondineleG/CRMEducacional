namespace CRMEducacional.API.Extensions;

public static class LoggerExtensions
{
    public static void CustomFormatLog(
        this ILogger logger,
        LogEventLevel level,
        string controllerName,
        string actionName,
        string messageTemplate,
        params object[] args
    )
    {
        var formattedMessageTemplate = $"{controllerName} => {actionName} => {messageTemplate}";

        switch (level)
        {
            case LogEventLevel.Verbose:
            logger.Verbose(formattedMessageTemplate, args);
            break;

            case LogEventLevel.Debug:
            logger.Debug(formattedMessageTemplate, args);
            break;

            case LogEventLevel.Information:
            logger.Information(formattedMessageTemplate, args);
            break;

            case LogEventLevel.Warning:
            logger.Warning(formattedMessageTemplate, args);
            break;

            case LogEventLevel.Error:
            logger.Error(formattedMessageTemplate, args);
            break;

            case LogEventLevel.Fatal:
            logger.Fatal(formattedMessageTemplate, args);
            break;

            default:
            logger.Information(formattedMessageTemplate, args);
            break;
        }
    }

    public static void RegisterLogServices(this IServiceCollection services)
    {
        services.AddSingleton(Log.Logger);
    }
}