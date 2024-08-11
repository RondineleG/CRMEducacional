namespace CRMEducacional.Core.Exceptions;

public class WebApiStartupException : Exception
{
    public WebApiStartupException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}